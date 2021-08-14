using Microsoft.Extensions.DependencyInjection;
using System;
using TempFileStream.Abstractions;
using TempFileStream.Infrastructure;

namespace TempFileStream
{
    /// <summary>
    /// temp file registerers
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// register <see cref="ITempFileFactory"/> along with a default suite of dependencies
        /// </summary>
        /// <param name="isc">service collection interface</param>
        /// <returns>the same service collection interface as was passed</returns>
        public static IServiceCollection AddTempFileStream(this IServiceCollection isc)
        {
            if (isc is null)
            {
                throw new ArgumentNullException(nameof(isc));
            }

            isc.AddSingleton<IReadStreamFactory, ReadStreamFactory>()
                .AddSingleton<IWriteStreamFactory, WriteStreamFactory>()
                .AddSingleton<IIOExceptionHandler, NullIOExceptionHandler>()
                .AddSingleton<ITempFolderInitializer, CheckOnlyTempFolderInitializer>()
                .AddSingleton<ITempFileFactory, TempFileFactory>()
                .AddSingleton<ITempFileNamer, TempFileNamer>()
                .AddSingleton<ITempFileStreamFactory, TempFileStreamFactory>()
                .AddSingleton<ITempFolderNamer, TempFolderNamer>()
                .AddSingleton<ITempFileDisposer, BackgroundTempFileDisposer>()
                .AddTransient<TempFileDependencies>();

            return isc;
        }
    }
}
