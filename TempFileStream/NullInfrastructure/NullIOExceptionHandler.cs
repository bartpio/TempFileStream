using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.NullInfrastructure
{
    /// <summary>
    /// temp file creation i/o exception handler that never calls for trying again
    /// </summary>
    public sealed class NullIOExceptionHandler : IIOExceptionHandler
    {
        /// <inheritdoc/>
        public bool ShouldTryAgain(IOException ioException, string fullFileName, int attemptIdx) => false;
    }
}
