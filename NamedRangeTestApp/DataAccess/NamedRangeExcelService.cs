using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.DataAccess.Common;
using NamedRangeTestApp.Exceptions;
using NamedRangeTestApp.Extensions;
using NamedRangeTestApp.Models;
using System;
using System.Collections.Generic;

namespace NamedRangeTestApp.DataAccess
{
    public class NamedRangeExcelService : ExcelService, INamedRangeExcelService
    {
        private readonly ILogger<NamedRangeExcelService> _logger;

        public NamedRangeExcelService(ILogger<NamedRangeExcelService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Cell> GetCellsByNamedRange(string namedRange)
        {
            using var package = InitPackage("Data", "testExcel.xlsx");

            var wb = package.Workbook;
            var cellRange = wb.Names[namedRange];

            var result = cellRange.GetCells();

            return result;
        }

        public void InsertValuesToNamedRange(string namedRange, string[] values)
        {
            using var package = InitPackage("Data", "testExcel.xlsx");

            var wb = package.Workbook;
            var cellRange = wb.Names[namedRange];

            try
            {
                cellRange.Insert(values);
                package.Save();
            }
            catch (NamedRangeInsertException ex)
            {
                _logger.LogInformation(string.Join(',', ex.Values));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
