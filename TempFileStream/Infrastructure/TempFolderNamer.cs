using Microsoft.Extensions.Options;
using System;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.Infrastructure
{
    /// <summary>
    /// temp folder namer, which honors <see cref="TempFileStreamConfig"/>, using <see cref="Path.GetTempPath"/> as default if nothing configured
    /// </summary>
    public class TempFolderNamer : ITempFolderNamer
    {
        private readonly TempFileStreamConfig _cfg;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="options"></param>
        public TempFolderNamer(IOptions<TempFileStreamConfig> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _cfg = options.Value;
        }

        /// <inheritdoc/>
        public string GetTempFolder()
        {
            if (!string.IsNullOrWhiteSpace(_cfg.RootFolder))
            {
                return _cfg.RootFolder;
            }
            else
            {
                return Path.GetTempPath();
            }
        }
    }
}
