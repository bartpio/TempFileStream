namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp folder initializer
    /// </summary>
    public interface ITempFolderInitializer
    {
        /// <summary>
        /// at the least, verifies that the specified temp root path exists
        /// some implementations may attempt to create the folder if it's missing
        /// implementation should throw <see cref="TempFileStreamFolderInitializationException"/> if the folder didn't exist or could not be created
        /// </summary>
        /// <param name="path">
        /// temp root path, for example /tmp
        /// </param>
        void InitializeRootFolder(string folder);
    }
}
