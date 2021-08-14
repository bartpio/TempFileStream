using System.IO;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp file read stream provider
    /// </summary>
    public interface IReadStreamFactory
    {
        /// <summary>
        /// create a stream used to read a temp file
        /// </summary>
        /// <param name="fullFileName">name of the temp file</param>
        /// <returns>a stream which can read the temp file</returns>
        Stream CreateReadStream(string fullFileName);
    }
}
