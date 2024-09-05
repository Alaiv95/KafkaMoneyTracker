using Infrastructure.FileUtils.dtos;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Core.fileUtils.dtos;

namespace Infrastructure.FileUtils.writers;

internal class ExcelWriter : IFileWriter
{
    private readonly ExcelInputInfo _inputInfo;
    private readonly XSSFWorkbook _workbook = new();
    private const string MimeExcelType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public ExcelWriter(ExcelInputInfo inputInfo)
    {
        _inputInfo = inputInfo;
    }

    public FileType Type { get; set; } = FileType.Excel;

    public FileWriteResultDto WriteFile()
    {
        ISheet sheet = _workbook.CreateSheet();
        AddRow(sheet, 0, _inputInfo.Header);

        for (var rowIndex = 1; rowIndex <= _inputInfo.Rows.Count; rowIndex++)
        {
            AddRow(sheet, rowIndex, _inputInfo.Rows[rowIndex - 1]);
        }

        using MemoryStream stream = new MemoryStream();
        _workbook.Write(stream);
        var data = stream.ToArray();

        return new FileWriteResultDto
        {
            MimeType = MimeExcelType,
            Content = data,
            FileName = $"{_inputInfo.Name}.xlsx"
        };
    }

    private void AddRow(ISheet sheet, int rowIndex, List<string> cellValues)
    {
        IRow row = sheet.CreateRow(rowIndex);

        for (var i = 0; i < cellValues.Count; i++)
        {
            var value = cellValues[i];
            row.CreateCell(i).SetCellValue(value);
        }
    }
}