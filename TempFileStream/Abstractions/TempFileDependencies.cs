using Microsoft.Extensions.Logging;
using System;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp file dependencies
    /// </summary>
    public class TempFileDependencies
    {
        /// <summary>
        /// construct, given various dependencies used to manage a temp file instance
        /// </summary>
        /// <param name="loggerFactory">for tracing</param>
        /// <param name="tempFileStreamFactory">for temp file creation</param>
        /// <param name="readStreamFactory">for temp file reading</param>
        /// <param name="tempFileDisposer">for temp file deletion</param>
        public TempFileDependencies(ILoggerFactory loggerFactory, ITempFileStreamFactory tempFileStreamFactory, IReadStreamFactory readStreamFactory, ITempFileDeleter tempFileDisposer)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            TempFileStreamFactory = tempFileStreamFactory ?? throw new ArgumentNullException(nameof(tempFileStreamFactory));
            ReadStreamFactory = readStreamFactory ?? throw new ArgumentNullException(nameof(readStreamFactory));
            TempFileDisposer = tempFileDisposer ?? throw new ArgumentNullException(nameof(tempFileDisposer));
        }

        /// <summary>
        /// for tracing
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// for temp file creation
        /// </summary>
        public ITempFileStreamFactory TempFileStreamFactory  { get; }

        /// <summary>
        /// for temp file reading
        /// </summary>
        public IReadStreamFactory ReadStreamFactory { get; }

        /// <summary>
        /// for temp file deletion
        /// </summary>
        public ITempFileDeleter TempFileDisposer { get; }
    }
}
