using System.IO;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp file creation i/o exception handler
    /// </summary>
    /// <remarks>
    /// this may be overkill; the provided NullIOExceptionHandler which simply never re-tries may be sufficient for all cases
    /// </remarks>
    public interface IIOExceptionHandler
    {
        /// <summary>
        /// given an exception during temp file creation and some information about the situation, should we re-attempt creating the same temp file?
        /// </summary>
        /// <param name="ioException">the i/o exception which was thrown during an attempt to create a temp file</param>
        /// <param name="fullFileName">name of the temp file we attempted to create</param>
        /// <param name="attemptIdx">the attempt number, starting with 0</param>
        /// <returns>true if we should try creating the temp file again</returns>
        bool ShouldTryAgain(IOException ioException, string fullFileName, int attemptIdx);
    }
}
