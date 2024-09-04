using Infrastructure.FileUtils.dtos;

namespace Infrastructure.FileUtils.writers;

public class FileWriterFactory
{
    private readonly Dictionary<FileType, Func<IInputInfo, IFileWriter>> _writers;

    public FileWriterFactory()
    {
        _writers = new Dictionary<FileType, Func<IInputInfo, IFileWriter>>
        {
            { FileType.Excel, info => new ExcelWriter((ExcelInputInputInfo)info) }
        };
    }

    public IFileWriter GetWriter(FileType type, IInputInfo inputInfo)
    {
        if (!_writers.TryGetValue(type, out var writer))
        {
            throw new ArgumentException($"Тип ${type.ToString()} не поддерживается");
        }

        return writer(inputInfo);
    }
}