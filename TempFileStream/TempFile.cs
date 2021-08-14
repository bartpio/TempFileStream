using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using TempFileStream.Abstractions;

namespace TempFileStream
{
    /// <summary>
    /// an instance of a temporary file, generally provided by an implementation of <see cref="ITempFileFactory"/>
    /// the <see cref="WriteStream"/> may be used immediately after instantion of a <see cref="TempFile"/> to populate the file
    /// the <see cref="WriteStream"/> can be disposed by the caller, after which <see cref="CreateReadStream"/> may be used to re-open the file for reading
    /// </summary>
    public sealed class TempFile : ITempFile, IDisposable
    {
        private readonly TempFileDependencies _deps;
        private readonly ILogger _logger;
        private readonly CancellationToken _ctoken;
        private readonly ConcurrentBag<Stream> _disposalbag = new ConcurrentBag<Stream>();

        /// <summary>
        /// construct temp file instance
        /// </summary>
        /// <param name="deps">various temp file operational dependencies</param>
        /// <param name="ctoken">token which may be used to interrupt temp file operations (including abandonment of any deletion attempt)</param>
        internal TempFile(TempFileDependencies deps, CancellationToken ctoken)
        {
            _deps = deps ?? throw new ArgumentNullException(nameof(deps));
            _logger = deps.LoggerFactory.CreateLogger<TempFile>();
            _ctoken = ctoken;
            (WriteStream, FullFileName) = CreateWriteStream(deps);
        }

        private static (Stream writeStream, string fullFileName) CreateWriteStream(TempFileDependencies deps) => deps.TempFileStreamFactory.CreateWriteStream();

        /// <inheritdoc/>
        public Stream WriteStream { get; }

        /// <inheritdoc/>
        public string FullFileName { get; }

        /// <inheritdoc/>
        public Stream CreateReadStream()
        {
            var readStream = _deps.ReadStreamFactory.CreateReadStream(FullFileName);
            _disposalbag.Add(readStream);
            return readStream;
        }

        /// <summary>
        /// delete the temp file (generally this will be done in a background task, asynchronously)
        /// </summary>
        public void Dispose()
        {
            WriteStream.Dispose();
            foreach (var entry in _disposalbag.ToList())
            {
                try
                {
                    entry.Dispose();
                }
                catch (Exception disposalException)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning(disposalException, "Failed to dispose one of the temp file read streams");
                    }
                }
            }

            _deps.TempFileDisposer.Delete(FullFileName, _ctoken);
        }
    }
}
