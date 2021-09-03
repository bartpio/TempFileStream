using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream
{
    /// <summary>
    /// temp file stream factory
    /// </summary>
    public class TempFileStreamFactory : ITempFileStreamFactory
    {
        private readonly ILogger _logger;
        private readonly ITempFileNamer _namer;
        private readonly IIOExceptionHandler _handler;
        private readonly IWriteStreamFactory _streams;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="namer"></param>
        /// <param name="handler"></param>
        /// <param name="streams"></param>
        public TempFileStreamFactory(ILogger<TempFileStreamFactory> logger, ITempFileNamer namer, IIOExceptionHandler handler, IWriteStreamFactory streams)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _namer = namer ?? throw new ArgumentNullException(nameof(namer));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _streams = streams ?? throw new ArgumentNullException(nameof(streams));
        }

        /// <inheritdoc/>
        public (Stream writeStream, string fullFileName) CreateWriteStream()
        {
            for (var attemptIdx = 0; attemptIdx < 1000; attemptIdx++)
            {
                var fullFileName = _namer.CreateTempFileName();
                try
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug($"Creating temp file {fullFileName} (attempt#{attemptIdx})");
                    }
                    return (_streams.CreateWriteStream(fullFileName), fullFileName);
                }
                catch (IOException ioe) when (_handler.ShouldTryAgain(ioe, fullFileName, attemptIdx))
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning(ioe, $"Creating write stream for temp file {fullFileName} didn't work, so we'll keep trying (#{attemptIdx})");
                    }
                }
            }

            throw new TempFileStreamException("Unable to create temp file");
        }
    }
}
