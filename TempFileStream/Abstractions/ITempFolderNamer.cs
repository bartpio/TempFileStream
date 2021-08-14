namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp folder namer
    /// </summary>
    public interface ITempFolderNamer
    {
        /// <summary>
        /// get the name of the root temp folder to use for temp file storage, for example "/tmp"
        /// </summary>
        /// <returns>for example "/tmp"</returns>
        string GetTempFolder();
    }
}
