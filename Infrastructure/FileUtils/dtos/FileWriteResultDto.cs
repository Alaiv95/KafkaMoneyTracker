namespace Infrastructure.FileUtils.dtos;

public class FileWriteResultDto
{
    public string FileName { get; init; }
    
    public string MimeType { get; init; }
    
    public byte[] Content { get; init; }
}