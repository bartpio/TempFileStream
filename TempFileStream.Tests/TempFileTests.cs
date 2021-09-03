using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TempFileStream.Abstractions;

namespace TempFileStream.Tests
{
    public class Tests
    {
        private ServiceProviderOptions ServiceProviderOptions => new ServiceProviderOptions() { ValidateScopes = true, ValidateOnBuild = true };

        [Test]
        public async Task TestOnDisk()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(lb =>
            {
                lb.SetMinimumLevel(LogLevel.Debug);
                lb.AddConsole(clo =>
                {
                    clo.DisableColors = true;
                });
            });
            sc.AddDiskBasedTempFileStream().Configure<TempFileStreamConfig>(cfg =>
              {
                  cfg.Prefix = "someunit_";
              });

            var buf = new byte[310_000];
            var rand = new Random();
            rand.NextBytes(buf);

            using (var sp = sc.BuildServiceProvider(ServiceProviderOptions))
            {
                var deleter = sp.GetRequiredService<ITempFileDeleter>();
                Assert.That(deleter.TotalDeleted, Is.Zero);

                var tempFileFactory = sp.GetRequiredService<ITempFileFactory>();

                FileInfo fi;
                using (var tempFile = tempFileFactory.CreateTempFile())
                {
                    fi = new FileInfo(tempFile.FullFileName);
                    Assert.That(fi.Exists, Is.True);
                    Assert.That(fi.Length, Is.Zero);
                    Assert.That(fi.Name, Does.StartWith("someunit_"));

                    // writeout temp
                    using (var ws = tempFile.WriteStream)
                    {
                        ws.Write(buf, 0, buf.Length);
                    }

                    fi.Refresh();
                    Assert.That(fi.Length, Is.EqualTo(buf.Length));

                    // now re-open it for reading etc.
                    using (var readStream = tempFile.CreateReadStream())
                    using (var ms = new MemoryStream())
                    {
                        readStream.CopyTo(ms);
                        var actualbuf = ms.ToArray();
                        Assert.That(actualbuf, Is.EqualTo(buf));
                    }
                }

                var sw = Stopwatch.StartNew();
                while (deleter.TotalDeleted <= 0 && sw.Elapsed < TimeSpan.FromSeconds(4))
                {
                    await Task.Delay(10).ConfigureAwait(false);
                }

                fi.Refresh();
                Assert.That(fi.Exists, Is.False);
            }
        }

        [Test]
        public async Task TestInMemory()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(lb =>
            {
                lb.SetMinimumLevel(LogLevel.Debug);
                lb.AddConsole(clo =>
                {
                    clo.DisableColors = true;
                });
            });
            sc.AddMemoryBasedTempFileStream().Configure<TempFileStreamConfig>(cfg =>
            {
                cfg.Prefix = "someunit_";
            });

            var buf = new byte[310_000];
            var rand = new Random();
            rand.NextBytes(buf);

            using (var sp0 = sc.BuildServiceProvider(ServiceProviderOptions))
            {
                using (var scope = sp0.CreateScope())
                {
                    var sp = scope.ServiceProvider;
                    var deleter = sp.GetRequiredService<ITempFileDeleter>();
                    Assert.That(deleter.TotalDeleted, Is.Zero);

                    var tempFileFactory = sp.GetRequiredService<ITempFileFactory>();

                    for (var idx = 0; idx < 10; idx++)
                    {
                        using (var tempFile = tempFileFactory.CreateTempFile())
                        {
                            TestContext.WriteLine("virtual inmemory filename is: {0}", tempFile.FullFileName);

                            // writeout temp
                            using (var ws = tempFile.WriteStream)
                            {
                                ws.Write(buf, 0, buf.Length);
                            }

                            // now re-open it for reading etc.
                            using (var readStream = tempFile.CreateReadStream())
                            using (var ms = new MemoryStream())
                            {
                                readStream.CopyTo(ms);
                                var actualbuf = ms.ToArray();
                                Assert.That(actualbuf, Is.EqualTo(buf));
                            }
                        }
                    }

                    Assert.That(deleter.TotalDeleted, Is.EqualTo(10));

                    var wsf = sp.GetRequiredService<InMemoryInfrastructure.InMemoryWriteStreamFactory>();
                    Assert.That(wsf.StreamCount, Is.EqualTo(10));
                }

                using (var anotherscope = sp0.CreateScope())
                {
                    var sp = anotherscope.ServiceProvider;
                    var wsf = sp.GetRequiredService<InMemoryInfrastructure.InMemoryWriteStreamFactory>();
                    Assert.That(wsf.StreamCount, Is.Zero);
                }
            }
        }
    }
}