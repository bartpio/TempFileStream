using System;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.InMemoryInfrastructure
{
    /// <summary>
    /// read stream factory geared towards inmemory use
    /// tightly coupled to <see cref="InMemoryWriteStreamFactory"/>
    /// </summary>
    public sealed class InMemoryReadStreamFactory : IReadStreamFactory
    {
        private readonly InMemoryWriteStreamFactory _writeStreamFactory;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="writeStreamFactory"></param>
        public InMemoryReadStreamFactory(InMemoryWriteStreamFactory writeStreamFactory)
        {
            _writeStreamFactory = writeStreamFactory ?? throw new ArgumentNullException(nameof(writeStreamFactory));
        }

        /// <inheritdoc/>
        public Stream CreateReadStream(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException($"'{nameof(fullFileName)}' cannot be null or whitespace.", nameof(fullFileName));
            }

            var buf = _writeStreamFactory.GetMemoryStream(fullFileName).ToArray();
            return new MemoryStream(buf, false);
        }
    }
}
