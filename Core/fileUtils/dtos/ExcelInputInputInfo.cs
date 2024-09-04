namespace Infrastructure.FileUtils.dtos;

public class ExcelInputInputInfo : IInputInfo
{
    public List<string> Header { get; set; }
    
    public List<List<string>> Rows { get; set; }
    
    public string Name { get; set; }
}