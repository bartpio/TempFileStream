using Microsoft.Extensions.DependencyInjection;
using System;
using TempFileStream.Abstractions;
using TempFileStream.Infrastructure;
using TempFileStream.InMemoryInfrastructure;
using TempFileStream.NullInfrastructure;

namespace TempFileStream
{
    /// <summary>
    /// temp file registerers
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// register <see cref="ITempFileFactory"/> along with a default suite of dependencies, for on-disk usage (the core use case)
        /// </summary>
        /// <param name="isc">service collection interface</param>
        /// <returns>the same service collection interface as was passed</returns>
        public static IServiceCollection AddDiskBasedTempFileStream(this IServiceCollection isc)
        {
            if (isc is null)
            {
                throw new ArgumentNullException(nameof(isc));
            }

            isc.AddCommon(ServiceLifetime.Singleton);

            isc.AddSingleton<IReadStreamFactory, ReadStreamFactory>()
                .AddSingleton<IWriteStreamFactory, WriteStreamFactory>()
                .AddSingleton<ITempFolderInitializer, CheckOnlyTempFolderInitializer>()
                .AddSingleton<ITempFileNamer, TempFileNamer>()
                .AddSingleton<ITempFolderNamer, TempFolderNamer>()
                .AddSingleton<ITempFileDeleter, BackgroundTempFileDeleter>();

            return isc;
        }

        /// <summary>
        /// register <see cref="ITempFileFactory"/> along with a default suite of dependencies, for on-disk usage (for usage when a writeable disk is not available)
        /// </summary>
        /// <param name="isc">service collection interface</param>
        /// <returns>the same service collection interface as was passed</returns>
        public static IServiceCollection AddMemoryBasedTempFileStream(this IServiceCollection isc)
        {
            if (isc is null)
            {
                throw new ArgumentNullException(nameof(isc));
            }

            // memory-based impl uses scoped lifetime of some infrastructure, because we don't want InMemoryWriteStreamFactory to hold on to an ever-growing bag of memorystreams
            isc.AddCommon(ServiceLifetime.Scoped);

            isc.AddScoped<IReadStreamFactory, InMemoryReadStreamFactory>()
                .AddScoped<IWriteStreamFactory, InMemoryWriteStreamFactory>()
                .AddSingleton<ITempFolderInitializer, NullTempFolderInitializer>()
                .AddSingleton<ITempFileNamer, SequentialTempFileNamer>()
                .AddSingleton<ITempFolderNamer, InMemoryTempFolderNamer>()
                .AddSingleton<ITempFileDeleter, NullTempFileDeleter>();

            isc.AddTransient(isp => (InMemoryWriteStreamFactory)isp.GetRequiredService<IWriteStreamFactory>());

            return isc;
        }

        private static IServiceCollection AddCommon(this IServiceCollection isc, ServiceLifetime lifetime)
        {
            isc.AddSingleton<IIOExceptionHandler, NullIOExceptionHandler>()
                .AddTransient<TempFileDependencies>();

            isc.Add<ITempFileFactory, TempFileFactory>(lifetime)
                .Add<ITempFileStreamFactory, TempFileStreamFactory>(lifetime);

            return isc;
        }

        private static IServiceCollection Add<TServiceType, TImplementationType>(this IServiceCollection isc, ServiceLifetime lifetime)
        {
            var sd = new ServiceDescriptor(typeof(TServiceType), typeof(TImplementationType), lifetime);
            isc.Add(sd);
            return isc;
        }
    }
}
