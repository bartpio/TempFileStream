using System.Globalization;
using System.Threading;
using TempFileStream.Abstractions;

namespace TempFileStream.InMemoryInfrastructure
{
    /// <summary>
    /// sequential temp file namer geared towards inmemory use
    /// </summary>
    /// <remarks>
    /// should generally be registered as singleton
    /// </remarks>
    public sealed class SequentialTempFileNamer : ITempFileNamer
    {
        private long _cnt;

        /// <summary>
        /// create a "temp file" name that is really just based on a sequential integer
        /// </summary>
        /// <returns>a "temp file" name</returns>
        public string CreateTempFileName()
        {
            var idx = Interlocked.Increment(ref _cnt);
            return idx.ToString(CultureInfo.InvariantCulture);
        }
    }
}
