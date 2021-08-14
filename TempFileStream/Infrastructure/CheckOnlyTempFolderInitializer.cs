using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TempFileStream.Abstractions;

namespace TempFileStream.Infrastructure
{
    /// <summary>
    /// a basic temp folder initializer which merely verifies that the configured folder exists
    /// </summary>
    public sealed class CheckOnlyTempFolderInitializer : ITempFolderInitializer
    {
        private readonly ILogger _logger;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="logger"></param>
        public CheckOnlyTempFolderInitializer(ILogger<CheckOnlyTempFolderInitializer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// check that the specified folder exists
        /// if not, throws <see cref="TempFileStreamFolderInitializationException"/>
        /// </summary>
        /// <param name="folder">temp root path, for example /tmp</param>
        public void InitializeRootFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentException($"'{nameof(folder)}' cannot be null or whitespace.", nameof(folder));
            }

            var di = new DirectoryInfo(folder);
            if (!di.Exists)
            {
                throw new TempFileStreamFolderInitializationException($"Temp folder '{folder}' does not exist");
            }
        }
    }
}
