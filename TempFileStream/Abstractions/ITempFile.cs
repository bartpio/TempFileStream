using System;
using System.IO;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// a disposable temporary file holder
    /// </summary>
    public interface ITempFile : IDisposable
    {
        /// <summary>
        /// access to write to the temp file, which is available immediately upon the temp file instance being constructed
        /// if caller is planning to re-open the file in read mode using <see cref="CreateReadStream"/>, <see cref="WriteStream"/> should be disposed by the caller after done writing to the temp file
        /// alternatively, caller can seek/read straight from this stream, and not use <see cref="CreateReadStream"/>
        /// </summary>
        Stream WriteStream { get; }

        /// <summary>
        /// name of the temp file
        /// </summary>
        string FullFileName { get; }
        
        /// <summary>
        /// get a stream to read the temp file
        /// the <see cref="WriteStream"/> should have been disposed prior to calling this
        /// </summary>
        /// <returns>a stream to read the temp file</returns>
        Stream CreateReadStream();
    }
}