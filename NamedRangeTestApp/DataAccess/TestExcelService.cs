using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.DataAccess.Common;
using NamedRangeTestApp.Exceptions;
using NamedRangeTestApp.Extensions;
using NamedRangeTestApp.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;

namespace NamedRangeTestApp.DataAccess
{
    public class TestExcelService : ExcelService, ITestExcelService
    {
        private readonly ILogger<TestExcelService> _logger;

        public TestExcelService(ILogger<TestExcelService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Cell> CheckNamedRangeReferences(string namedRange, string[] values)
        {
            using var scenarioPackage = InitPackage("Data/testSet", "scenario.xlsx");

            var scenarioWb = scenarioPackage.Workbook;
            var scenarioCellRange = scenarioWb.Names[namedRange];

            try
            {
                scenarioCellRange.Insert(values);
            }
            catch (NamedRangeInsertException ex)
            {
                _logger.LogInformation(string.Join(',', ex.Values));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            scenarioPackage.Save();

            using var testCalcPackage = InitPackage("Data/testSet", "test_calc.xlsx");

            var testCalcWb = testCalcPackage.Workbook;

            testCalcWb.CalcMode = ExcelCalcMode.Manual;
            testCalcWb.Calculate();
            testCalcPackage.Save();

            var testCalcCellRange = testCalcWb.Names[namedRange];

            var result = testCalcCellRange.GetCells();

            return result;
        }
    }
}
