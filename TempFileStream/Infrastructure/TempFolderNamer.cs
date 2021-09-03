using Microsoft.Extensions.Options;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.Infrastructure
{
    /// <summary>
    /// temp folder namer, which honors <see cref="TempFileStreamConfig"/>, using <see cref="Path.GetTempPath"/> as default if nothing configured
    /// </summary>
    public class TempFolderNamer : TempFolderNamerBase
    {
        /// <summary>
        /// construct
        /// </summary>
        /// <param name="options"></param>
        public TempFolderNamer(IOptions<TempFileStreamConfig> options) : base(options)
        {
        }

        /// <summary>
        /// get temp folder (for use when not otherwise configured) using a call to <see cref="Path.GetTempPath"/>
        /// </summary>
        /// <returns>temp folder (for use when not otherwise configured) using a call to <see cref="Path.GetTempPath"/></returns>
        public override string GetFallbackTempFolder()
        {
            return Path.GetTempPath();
        }
    }
}
