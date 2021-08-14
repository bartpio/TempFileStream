using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TempFileStream.Abstractions;

namespace TempFileStream.Infrastructure
{
    /// <summary>
    /// background temp file disposer
    /// launches a task for each temp file to be deleted, then returns straight away
    /// </summary>
    public class BackgroundTempFileDisposer : ITempFileDisposer
    {
        private readonly ILogger _logger;
        private long _totalDeleted;

        /// <inheritdoc/>
        public long TotalDeleted => _totalDeleted;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="logger"></param>
        public BackgroundTempFileDisposer(ILogger<BackgroundTempFileDisposer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// launch a task which shall delete the specified temp file
        /// </summary>
        /// <param name="fullFileName">temp file we'll be deleting</param>
        /// <param name="ctoken">cancellation token which can be used to abandon deletion attempt</param>
        public void Delete(string fullFileName, CancellationToken ctoken)
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException($"'{nameof(fullFileName)}' cannot be null or whitespace.", nameof(fullFileName));
            }

            var logWarnings = _logger.IsEnabled(LogLevel.Warning);
            var logDebug = _logger.IsEnabled(LogLevel.Debug);
            var sw = Stopwatch.StartNew();
            try
            {
                Task.Run(async () =>
                {
                    await Task.Yield();
                    for (var idx = 0; idx < 3; idx++)
                    {
                        try
                        {
                            File.Delete(fullFileName);
                            
                            try
                            {
                                sw.Stop();
                                Interlocked.Increment(ref _totalDeleted);
                                if (logDebug)
                                {
                                    _logger.LogDebug($"Deleted temp file '{fullFileName}' (took {sw.ElapsedMilliseconds}ms)");
                                }
                            }
                            catch (Exception)
                            {
                                // do nothing here
                                // overly protective catch block for the logdebug here, to avoid false Failed to delete temp file warnings
                            }

                            break;
                        }
                        catch (Exception exc)
                        {
                            if (logWarnings)
                            {
                                _logger.LogWarning(exc, $"Failed to delete temp file '{fullFileName}' (attempt #{idx})");
                            }
                            await Task.Delay(100, ctoken).ConfigureAwait(false);
                        }
                    }
                }, ctoken);
            }
            catch (TaskCanceledException tcexc)
            {
                // not a common case at all
                if (logWarnings)
                {
                    _logger.LogWarning(tcexc, $"Deletion of temp file '{fullFileName}' canceled");
                }
            }
        }
    }
}
