namespace Core.fileUtils;

public interface IFileReader
{
    public Task<string> GetFileDataAsync(string file);
}