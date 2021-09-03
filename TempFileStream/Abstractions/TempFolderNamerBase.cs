using Microsoft.Extensions.Options;
using System;
using TempFileStream.Abstractions;

namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp folder namer abstract base, which honors <see cref="TempFileStreamConfig"/>, using <see cref="Path.GetTempPath"/> as default if nothing configured
    /// </summary>
    public abstract class TempFolderNamerBase : ITempFolderNamer
    {
        private readonly TempFileStreamConfig _cfg;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="options"></param>
        public TempFolderNamerBase(IOptions<TempFileStreamConfig> options)
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
                return GetFallbackTempFolder();
            }
        }

        /// <summary>
        /// get temp folder name in the event that one hasn't been configured
        /// </summary>
        /// <returns>temp folder name to use in the event that one hasn't been configured</returns>
        public abstract string GetFallbackTempFolder();
    }
}
