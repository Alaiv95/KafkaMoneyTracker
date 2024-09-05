using Core.fileUtils;

namespace Infrastructure.FileUtils.readers;

public class BaseFileReader : IFileReader
{
    public async Task<string> GetFileDataAsync(string file)
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), file);

        if (!File.Exists(path))
        {
            return String.Empty;
        }

        using StreamReader reader = new StreamReader(path);
        var result = await reader.ReadToEndAsync();

        return result;
    }
}