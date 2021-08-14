using Microsoft.Extensions.Options;
using System;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.Infrastructure
{
    /// <summary>
    /// write stream factory, which honors any stream configuration supplied by <see cref="TempFileStreamConfig"/>
    /// </summary>
    public class WriteStreamFactory : IWriteStreamFactory
    {
        private readonly TempFileStreamConfig _cfg;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="options"></param>
        public WriteStreamFactory(IOptions<TempFileStreamConfig> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _cfg = options.Value;
        }

        /// <inheritdoc/>
        public Stream CreateWriteStream(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException($"'{nameof(fullFileName)}' cannot be null or whitespace.", nameof(fullFileName));
            }

            return new FileStream(fullFileName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read, _cfg.InternalGetBufferSize(), _cfg.InternalGetFileOptions());
        }
    }
}
