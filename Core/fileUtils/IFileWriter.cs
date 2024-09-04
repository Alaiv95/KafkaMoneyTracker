using Infrastructure.FileUtils.dtos;

namespace Infrastructure.FileUtils.writers;

public interface IFileWriter
{
    public FileWriteResultDto WriteFile();
    
    public FileType Type { get; set; }
}