using System;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// represents an exception initializing (ex. verifying the existence of) temp folder
    /// </summary>
    public class TempFileStreamFolderInitializationException : TempFileStreamException
    {
        /// <summary>
        /// construct exception
        /// </summary>
        public TempFileStreamFolderInitializationException()
        {
        }

        /// <inheritdoc/>
        public TempFileStreamFolderInitializationException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public TempFileStreamFolderInitializationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
