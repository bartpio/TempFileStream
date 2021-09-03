using Microsoft.Extensions.Options;
using TempFileStream.Abstractions;

namespace TempFileStream.InMemoryInfrastructure
{
    /// <summary>
    /// temp folder namer, which honors <see cref="TempFileStreamConfig"/> (although for in-memory impl the "folder" names an internal dictionary key prefix of no real consequence)
    /// </summary>
    public sealed class InMemoryTempFolderNamer : TempFolderNamerBase
    {
        /// <summary>
        /// construct
        /// </summary>
        /// <param name="options"></param>
        public InMemoryTempFolderNamer(IOptions<TempFileStreamConfig> options) : base(options)
        {
        }

        /// <summary>
        /// fallback tempfolder for inmemory is empty string, as real filesystem folders are not implay
        /// </summary>
        /// <returns></returns>
        public override string GetFallbackTempFolder() => string.Empty;
    }
}
