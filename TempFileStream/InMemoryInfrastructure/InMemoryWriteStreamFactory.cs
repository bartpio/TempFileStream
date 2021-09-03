using System;
using System.Collections.Concurrent;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.InMemoryInfrastructure
{
    /// <summary>
    /// write stream factory geared towards inmemory use
    /// creates instances of <see cref="MemoryStream"/>, and holds on to them to pull readable buffers
    /// </summary>
    /// <remarks>
    /// should generally be registered as scoped (registering as singleton will result in ever-increasing memory usage)
    /// </remarks>
    public sealed class InMemoryWriteStreamFactory : IWriteStreamFactory
    {
        private readonly ConcurrentDictionary<string, MemoryStream> _streams = new ConcurrentDictionary<string, MemoryStream>();

        /// <summary>
        /// provide access to stored stream count, primarily for testing purposes
        /// </summary>
        public int StreamCount => _streams.Count;

        /// <inheritdoc/>
        public Stream CreateWriteStream(string fullFileName)
        {
            return _streams.GetOrAdd(fullFileName, key =>
            {
                return new MemoryStream();
            });
        }

        /// <summary>
        /// provide access to written-to streams, so that <see cref="InMemoryReadStreamFactory"/> can pull their buffers
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <returns>written-to streams, from which buffer can be pulled</returns>
        internal MemoryStream GetMemoryStream(string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException($"'{nameof(fullFileName)}' cannot be null or whitespace.", nameof(fullFileName));
            }

            if (_streams.TryGetValue(fullFileName, out var ms))
            {
                return ms;
            }
            else
            {
                throw new FileNotFoundException($"InMemoryInfrastructure virtual file not found: {fullFileName}");
            }
        }
    }
}
