using System;
using System.IO;

namespace TempFileStream
{
    internal static class FileOptionsAdapter
    {
        public static FileOptions AdaptFileOptions(string fileOptions)
        {
            if (string.IsNullOrWhiteSpace(fileOptions))
            {
                return FileOptions.None;
            }
            else
            {
                if (Enum.TryParse<FileOptions>(fileOptions, out var result))
                {
                    return result;
                }
                else
                {
                    return FileOptions.None;
                }
            }
        }
    }
}
