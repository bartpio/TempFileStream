using System.IO;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp file stream factory
    /// </summary>
    public interface ITempFileStreamFactory
    {
        /// <summary>
        /// mint a new temp file
        /// </summary>
        /// <returns>
        /// 1) a stream providing write access to the newly minted file
        /// 2) the full path to the newly minted file
        /// </returns>
        (Stream writeStream, string fullFileName) CreateWriteStream();
    }
}
