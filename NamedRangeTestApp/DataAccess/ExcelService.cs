using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace NamedRangeTestApp.DataAccess
{
    public class ExcelService : IExcelService
    {
        private readonly ILogger<ExcelService> _logger;

        public ExcelService(ILogger<ExcelService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Cell> GetCellsByNamedRange(string namedRange)
        {
            var dir = Directory.GetCurrentDirectory();
            var file = new FileInfo(dir + "/Data/testExcel.xlsx");

            if (!file.Exists)
                return null;

            using var package = new ExcelPackage(file);

            var wb = package.Workbook;
            var cellRange = wb.Names[namedRange];

            var colNum = cellRange.Columns;
            var rowNum = cellRange.Rows;
            var startCol = cellRange.Start.Column;
            var startRow = cellRange.Start.Row;

            var result = new List<Cell>(colNum * rowNum);

            for (int i = startRow; i < (startRow + rowNum); i++)
            {
                for (int j = startCol; j < (startCol + colNum); j++)
                {
                    var cell = cellRange.Worksheet.Cells[i, j];

                    var str = $"{cell.Address}: {cell.Value}";
                    result.Add(new Cell
                    {
                        Address = cell.Address,
                        Value = cell.Value.ToString(),
                    });
                }
            }

            return result;
        }
    }
}
