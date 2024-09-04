using System.Runtime.Serialization;

namespace Infrastructure.FileUtils;

public enum FileType
{
    [EnumMember(Value = "excel")]
    Excel = 1,
    
    [EnumMember(Value = "pdf")]
    Pdf = 2
}