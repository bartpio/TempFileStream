using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using TempFileStream.Abstractions;

namespace TempFileStream
{
    /// <summary>
    /// provider of disposable <see cref="ITempFile"/> instances
    /// used to create new temp files
    /// </summary>
    public class TempFileFactory : ITempFileFactory
    {
        private readonly TempFileDependencies _deps;
        private readonly ILogger _logger;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="deps"></param>
        public TempFileFactory(TempFileDependencies deps)
        {
            _deps = deps ?? throw new ArgumentNullException(nameof(deps));
            _logger = deps.LoggerFactory.CreateLogger<TempFileFactory>();
        }

        /// <inheritdoc/>
        public ITempFile CreateTempFile(CancellationToken ctoken = default) => new TempFile(_deps, ctoken);
    }
}
