using Microsoft.Extensions.Options;
using System;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.Infrastructure
{
    /// <summary>
    /// temp file read stream provider that honors stream settings specified using <see cref="TempFileStreamConfig"/>
    /// </summary>
    public sealed class ReadStreamFactory : IReadStreamFactory
    {
        private readonly TempFileStreamConfig _cfg;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="options">options regarding temp files that we'll create</param>
        public ReadStreamFactory(IOptions<TempFileStreamConfig> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _cfg = options.Value;
        }

        /// <inheritdoc/>
        public Stream CreateReadStream(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException($"'{nameof(fullFileName)}' cannot be null or whitespace.", nameof(fullFileName));
            }

            return new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete, _cfg.InternalGetBufferSize(), _cfg.InternalGetFileOptions());
        }
    }
}
