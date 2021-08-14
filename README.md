# Temp File Stream
Makes using disk-backed temporary storage almost as simple as using a MemoryStream. Implements IDisposable.
Geared towards net core services using DI (IServiceProvider).

## Why?
Disk-based temporary storage is often under-utilized. Consider that some cloud providers provide fast ephemeral storage
free of additional charge!

## Commonly used Interfaces
To create a temp file, use ITempFileFactory's CreateTempFile method. This provides an instance of
ITempFile, with its WriteStream property available for immediate use. Write information to the
WriteStream, then dispose the WriteStream. To re-open the same temp file for reading, call
CreateReadStream on the ITempFile. When done working with the temp file, dispose  the read stream,
and then dispose the ITempFile instance.

## Temp File naming
Temp file names incorporate a cryptograhpically sound unique identifier, when the default namer
(TempFileNamer) is used. This default namer is registered as part of the call to 
services.AddTempFileStream.

## Temp File deletion
The default temp file disposer (BackgroundTempFileDisposer) launches a task (using Task.Run) when
an ITempFile is disposed, deleting the file in the background. If deletion fails, several more 
attempts are made. The default implementation usually does the job, but does not attempt to offer
a strong guarantee of temp file deletion. In particular, temp files created towards the end of the
app lifetime might fail to be deleted. This strategy is geared towards apps running in a container
(with a writeable temp folder!).

## Configuration
The root folder where temp files are placed, file name prefix, and stream behaviors may be configured
using TempFileStreamConfig. By default, Path.GetTempPath is used, and temp file names bear no particular prefix. 
The default configuration is suitable for many use cases.

## Interfaces controlling implementation details
A suite of interfaces, in the TempFileStream.Abstractions namespace, can be used to control various
facets of temporary file operations. The default implementations, as registered by
services.AddTempFileStream, are suitable for most use cases. Note that custom implementations of
some of the interfaces could in theory choose to not honor TempFileStreamConfig.

## DI Registration
```csharp
// without any particular configuration (suitable for many use cases)
services.AddTempFileStream();

// with custom configuration
services.AddTempFileStream().Configure<TempFileStreamConfig>(cfg =>
              {
                  cfg.Prefix = "someunit_";
              });

```
## Example Usage
```csharp
var tempFileFactory = serviceProvider.GetRequiredService<ITempFileFactory>();
using (var tempFile = tempFileFactory.CreateTempFile())
{
    // writeout data to temp file's write stream, and dispose the write stream
    using (var ws = tempFile.WriteStream)
    {
        ws.Write(buf, 0, buf.Length);
    }

    // now re-open it for reading etc.
    using (var readStream = tempFile.CreateReadStream())
    {
        // ...
    }
}
```

## Security
The default implementation does incorporate a cryptographically sound random value in temp file names,
but other than that, makes no further attempt to deter enumeration of temp files, access to their contents, 
or tampering with the contents. For some applications, it would be a good idea to use a generated in-memory
symmetric key to encrypt temp file contents, effectively providing a confidentiality guarantee as good as
straight in-memory storage of temporary data. Future versions of the library may offer such functionality.