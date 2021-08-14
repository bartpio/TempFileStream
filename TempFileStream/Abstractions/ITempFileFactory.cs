using System.Threading;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// provider of disposable <see cref="ITempFile"/> instances
    /// used to create new temp files
    /// </summary>
    public interface ITempFileFactory
    {
        /// <summary>
        /// create a new temp file
        /// </summary>
        /// <param name="ctoken">cancellation token which may be used to abandon stream creation or deletion of the temp file</param>
        /// <returns>an object providing access to the newly minted temp file</returns>
        ITempFile CreateTempFile(CancellationToken ctoken = default);
    }
}