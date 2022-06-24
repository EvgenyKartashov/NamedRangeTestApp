using Core.Logging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.DataAccess.Common;
using NamedRangeTestApp.Exceptions;
using NamedRangeTestApp.Extensions;
using NamedRangeTestApp.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace NamedRangeTestApp.DataAccess;

public class TestExcelService : ExcelService, ITestExcelService
{
    private readonly ILogger<TestExcelService> _logger;
    private readonly string _baseFolder;
    private readonly string _scenarioFileName;
    private readonly string _calcFileName;
    private readonly string _configFileName;

    public TestExcelService(ILogger<TestExcelService> logger, IConfiguration config)
    {
        _logger = logger;

        _baseFolder = config.GetValue<string>("Files:BaseFolder");
        _scenarioFileName = config.GetValue<string>("Files:Scenario");
        _calcFileName = config.GetValue<string>("Files:Calc");
        _configFileName = config.GetValue<string>("Files:Config");
    }

    public void AddValuesToScenario(IEnumerable<NamedRangeData> data)
    {
        using var _scenarioPackage = InitPackage(_baseFolder, _scenarioFileName);

        var scenarioWb = _scenarioPackage.Workbook;

        foreach (var namedRangeData in data)
        {
            var namedRange = namedRangeData.NamedRange;
            var values = namedRangeData.Values;

            var scenarioCellRange = scenarioWb.Names.First(range => range.Name == namedRange);

            try
            {
                scenarioCellRange.Insert(values);
            }
            catch (NamedRangeInsertException ex)
            {
                _logger.Info(string.Join(',', ex.Values))
                       .Write();
            }
        }

        _scenarioPackage.Save();
    }

    public Cell[] RecalculateModel()
    {
        using var _scenarioPackage = InitPackage(_baseFolder, _scenarioFileName);

        var scenarioWb = _scenarioPackage.Workbook;


        //todo: here, parse json
        var scenarioCellRange = scenarioWb.Names[namedRange];

        var values = scenarioCellRange.GetCells()
            .Select(cell => cell.Value)
            .ToArray();

        using var testCalcPackage = InitPackage(_baseFolder, _calcFileName);

        var calcWb = testCalcPackage.Workbook;
        var calcCellRange = calcWb.Names.First(range => range.Name == namedRange);

        try
        {
            calcCellRange.Insert(values);
        }
        catch (NamedRangeInsertException ex)
        {
            _logger.LogInformation(string.Join(',', ex.Values));
        }

        calcWb.Calculate();
        testCalcPackage.Save();

        var result = calcCellRange.GetCells().ToArray();

        return result;
    }
}
