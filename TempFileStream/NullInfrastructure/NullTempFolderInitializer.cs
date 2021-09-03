using TempFileStream.Abstractions;

namespace TempFileStream.InMemoryInfrastructure
{
    /// <summary>
    /// temp folder initializer that does nothing
    /// </summary>
    public sealed class NullTempFolderInitializer : ITempFolderInitializer
    {
        /// <summary>
        /// do absolutely nothing
        /// </summary>
        /// <param name="folder"></param>
        public void InitializeRootFolder(string folder)
        {
        }
    }
}
