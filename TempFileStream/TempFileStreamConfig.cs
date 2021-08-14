using System.IO;

namespace TempFileStream
{
    /// <summary>
    /// temp file subsystem configuration
    /// </summary>
    public class TempFileStreamConfig
    {
        /// <summary>
        /// root folder
        /// generally defaults to Path.GetTempPath
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// temp file name prefix
        /// defaults to no prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// buffer size to specify when creating a <see cref="System.IO.FileStream"/>
        /// the default is 4096
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        /// internal convenience method to get buffer size or default
        /// yea this responsibility maybe belongs somewhere else
        /// </summary>
        /// <returns>
        /// buffer size to use
        /// always greater than 0
        /// </returns>
        internal int InternalGetBufferSize() => (BufferSize > 0) ? BufferSize : 4096;

#if NOT_DEFINED__USE_FILE_OPTIONS_INSTEAD
        /// <summary>
        /// should we enable async i/o when creating files?
        /// please <see cref="System.IO.FileStream.FileStream(Microsoft.Win32.SafeHandles.SafeFileHandle, System.IO.FileAccess, int, bool)"/>
        /// </summary>
        public bool UseAsync { get; set; }
#endif

        /// <summary>
        /// optional comma delimited list specifying zero or more of these options: WriteThrough,Encrypted,DeleteOnClose,SequentialScan,RandomAccess,Asynchronous
        /// please see <see cref="System.IO.FileOptions"/>
        /// </summary>
        public string FileOptions { get; set; }

        /// <summary>
        /// internal convenience method to get enum-flags form of <see cref="System.IO.FileOptions"/>
        /// yea it could be an extension method or something and maybe doesn't belong here
        /// </summary>
        /// <returns>
        /// an instance of <see cref="System.IO.FileOptions"/> based on the configured comma delimited <see cref="FileOptions"/>
        /// </returns>
        internal FileOptions InternalGetFileOptions() => FileOptionsAdapter.AdaptFileOptions(FileOptions);
    }
}
