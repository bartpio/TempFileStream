using System.IO;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// write stream factory
    /// </summary>
    public interface IWriteStreamFactory
    {
        /// <summary>
        /// create a stream used to write to a particular temp file (for which the name has already been obtained)
        /// </summary>
        /// <param name="fullFileName">name of the temp file we're about to mint</param>
        /// <returns>stream providing write access</returns>
        Stream CreateWriteStream(string fullFileName);
    }
}
