using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.Exceptions;
using NamedRangeTestApp.Extensions;
using NamedRangeTestApp.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace NamedRangeTestApp.DataAccess
{
    public class TestExcelService : IExcelService
    {
        private readonly ILogger<TestExcelService> _logger;

        public TestExcelService(ILogger<TestExcelService> logger)
        {
            _logger = logger;
        }

        private static ExcelPackage InitPackage(string subdir, string fileName)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var file = new FileInfo($"{currentDir}/{subdir}/{fileName}");

            if (!file.Exists)
                throw new Exception("File does not exist");

            return new ExcelPackage(file);
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
            }
            catch (NamedRangeInsertException ex)
            {
            }

            package.Save();
        }
    }
}
