using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Security.Cryptography;
using TempFileStream.Abstractions;

namespace TempFileStream.Infrastructure
{
    /// <summary>
    /// temp file namer which incorporates a cryptographically sound random value,
    /// and optionally a prefix specified using <see cref="TempFileStreamConfig"/>
    /// </summary>
    public class TempFileNamer : ITempFileNamer
    {
        private readonly ITempFolderNamer _tempFolderNamer;
        private readonly ITempFolderInitializer _rootFolderInitializer;
        private readonly TempFileStreamConfig _cfg;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="tempFolderNamer"></param>
        /// <param name="rootFolderInitializer"></param>
        /// <param name="cfg"></param>
        public TempFileNamer(ITempFolderNamer tempFolderNamer, ITempFolderInitializer rootFolderInitializer, IOptions<TempFileStreamConfig> cfg)
        {
            if (cfg is null)
            {
                throw new ArgumentNullException(nameof(cfg));
            }

            _tempFolderNamer = tempFolderNamer ?? throw new ArgumentNullException(nameof(tempFolderNamer));
            _rootFolderInitializer = rootFolderInitializer ?? throw new ArgumentNullException(nameof(rootFolderInitializer));
            _cfg = cfg.Value;
        }

        /// <summary>
        /// create file name formatted like <see cref="Guid.ToString"/> (but unlike a run of the mill <see cref="Guid"/>, the value used IS cryptographically sound)
        /// optionally, incorporates prefix specified using <see cref="TempFileStreamConfig"/>
        /// </summary>
        /// <returns>full path to a proposed temp file</returns>
        public string CreateTempFileName()
        {
            var tempFolder = _tempFolderNamer.GetTempFolder();
            _rootFolderInitializer.InitializeRootFolder(tempFolder);

            var buf = new byte[16];
            RNG.GetBytes(buf);
            var guid = new Guid(buf);
            var namepart = (_cfg.Prefix ?? string.Empty) + guid.ToString();
            var result = Path.Combine(tempFolder, namepart);
            if (File.Exists(result))
            {
                // this will never, ever happen
                throw new TempFileStreamException($"Name conflict during temp file creation attempt (name attempted: '{result}')");
            }
            return result;
        }

        private static RandomNumberGenerator RNG { get; } = RandomNumberGenerator.Create();
    }
}
