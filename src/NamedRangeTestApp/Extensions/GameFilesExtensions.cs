using Microsoft.Extensions.Configuration;
using NamedRangeTestApp.DataAccess.Common;
using NamedRangeTestApp.Exceptions;
using OfficeOpenXml;
using System.Linq;

namespace NamedRangeTestApp.Extensions
{
    internal static class GameFilesExtensions
    {
        internal static ExcelPackage InsertValuesToScenario(IConfiguration config, string namedRange, string[] values)
        {
            var baseFolder = config.GetValue<string>("Files:BaseFolder");
            var scenarioFileName = config.GetValue<string>("Files:Scenario");

            var scenarioPackage = ExcelService.InitPackage(baseFolder, scenarioFileName);

            var scenarioWb = scenarioPackage.Workbook;
            var scenarioCellRange = scenarioWb.Names.First(range => range.Name == namedRange);

            try 
            {            
                scenarioCellRange.Insert(values);
            }
            catch (NamedRangeInsertException ex)
            {
                //_logger.LogInformation(string.Join(',', ex.Values));
            }

            scenarioPackage.Save();

            return scenarioPackage;
        }

        internal static ExcelPackage InsertValuesToCalc(this ExcelPackage scenarioPackage, IConfiguration config, string namedRange)
        {
            var baseFolder = config.GetValue<string>("Files:BaseFolder");
            var calcFileName = config.GetValue<string>("Files:Calc");

            var scenarioWb = scenarioPackage.Workbook;
            var scenarioCellRange = scenarioWb.Names[namedRange];

            var values = scenarioCellRange.GetCells()
                .Select(cell => cell.Value)
                .ToArray();

            using var testCalcPackage = ExcelService.InitPackage(baseFolder, calcFileName);

            var calcWb = testCalcPackage.Workbook;
            var calcCellRange = scenarioWb.Names.First(range => range.Name == namedRange);

            calcCellRange.Insert(values);
            calcWb.Calculate();
            testCalcPackage.Save();

            return scenarioPackage;
        }

        internal static void DisposeScenarioPackage(this ExcelPackage scenarioPackage)
        {
            scenarioPackage.Dispose();
        }
    }
}
