using System;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// represents an exception regarding temp file operations
    /// </summary>
    public class TempFileStreamException : Exception
    {
        /// <summary>
        /// construct exception
        /// </summary>
        public TempFileStreamException()
        {
        }

        /// <inheritdoc/>
        public TempFileStreamException(string message)
            : base(message)
        {
        }

        /// <inheritdoc/>
        public TempFileStreamException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
