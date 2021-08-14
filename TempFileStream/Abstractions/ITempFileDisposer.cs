using System.Threading;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp file disposer
    /// deletes temp files when we no longer need them
    /// </summary>
    public interface ITempFileDisposer
    {
        /// <summary>
        /// delete a temp file
        /// </summary>
        /// <param name="fullFileName">name of the temp file</param>
        /// <param name="ctoken">cancellation token which can be used to abandon deletion attempt</param>
        void Delete(string fullFileName, CancellationToken ctoken);
        
        /// <summary>
        /// total number of files that have ever been succesfully deleted by this instance of temp file disposer
        /// </summary>
        long TotalDeleted { get; }
    }
}
