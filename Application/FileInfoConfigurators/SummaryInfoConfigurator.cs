﻿using System.Globalization;
using Application.common.FileInfoConfigurators;
using Application.Dtos;
using Infrastructure.FileUtils;
using Infrastructure.FileUtils.dtos;

namespace Application.FileInfoConfigurators;

public class SummaryInfoConfigurator : IConfigurator<List<TransactionSummaryDto>>
{
    public IInputInfo Configure(FileType type, List<TransactionSummaryDto> data)
    {
        return type switch
        {
            FileType.Excel => ConfigureExcelInfo(data),
            _ => throw new ArgumentException($"Тип ${type.ToString()} не поддерживается")
        };
    }

    private ExcelInputInputInfo ConfigureExcelInfo(List<TransactionSummaryDto> data)
    {
        var header = new List<string> { "CategoryName", "Expenses", "Income" };

        var rowsData = data.Select(info => new List<string>
        {
            info.CategoryName,
            info.Expenses.ToString(CultureInfo.InvariantCulture),
            info.Income.ToString(CultureInfo.InvariantCulture)
        }).ToList();

        return new ExcelInputInputInfo
        {
            Rows = rowsData,
            Header = header,
            Name = Guid.NewGuid().ToString()
        };
    }
}