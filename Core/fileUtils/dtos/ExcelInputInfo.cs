namespace Core.fileUtils.dtos;

public class ExcelInputInfo : IInputInfo
{
    public List<string> Header { get; set; }
    
    public List<List<string>> Rows { get; set; }
    
    public string Name { get; set; }
}