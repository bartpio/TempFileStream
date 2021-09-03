using System.Threading;
using TempFileStream.Abstractions;

namespace TempFileStream.NullInfrastructure
{
    /// <summary>
    /// tempfile disposer that does very little
    /// </summary>
    public sealed class NullTempFileDeleter : ITempFileDeleter
    {
        private long _totalDeleted;

        /// <inheritdoc/>
        public long TotalDeleted => _totalDeleted;

        /// <summary>
        /// do almost nothing (just track invocation count for the potential benefit of some kinds of unit tests)
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="ctoken"></param>
        public void Delete(string fullFileName, CancellationToken ctoken)
        {
            _totalDeleted++;
        }
    }
}
