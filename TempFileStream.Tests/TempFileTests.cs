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
        [Test]
        public async Task TestTemporaryFileUsage()
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
            sc.AddTempFileStream().Configure<TempFileStreamConfig>(cfg =>
              {
                  cfg.Prefix = "someunit_";
              });

            var buf = new byte[310_000];
            var rand = new Random();
            rand.NextBytes(buf);

            using (var sp = sc.BuildServiceProvider())
            {
                var deleter = sp.GetRequiredService<ITempFileDisposer>();
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
    }
}