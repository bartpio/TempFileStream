namespace TempFileStream.Abstractions
{
    /// <summary>
    /// temp file namer
    /// </summary>
    public interface ITempFileNamer
    {
        /// <summary>
        /// create a name to use for a newly minted temp file
        /// </summary>
        /// <returns>full path to be used for the new temp file (generally, this doesn't yet exist at the time this method is returning)</returns>
        string CreateTempFileName();
    }
}
