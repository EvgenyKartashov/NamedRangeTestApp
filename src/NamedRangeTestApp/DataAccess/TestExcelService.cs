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
using System.IO;
using System.Linq;
using System.Text.Json;

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
        using var scenarioPackage = InitPackage(_baseFolder, _scenarioFileName);

        var scenarioWb = scenarioPackage.Workbook;

        foreach (var namedRangeData in data)
        {
            var namedRange = namedRangeData.NamedRange;
            var values = namedRangeData.Values;

            var scenarioCellRange = scenarioWb.Names.First(range => range.Name == namedRange);

            try
            {
                scenarioCellRange.Insert(values.Cast<object>().ToArray());
            }
            catch (NamedRangeInsertException ex)
            {
                _logger.Info(string.Join(',', ex.Values))
                       .Write();
            }
        }

        scenarioPackage.Save();
    }

    //todo: test inserting and calculation
    public void RecalculateModel()
    {
        using var scenarioPackage = InitPackage(_baseFolder, _scenarioFileName);
        using var modelPackage = InitPackage(_baseFolder, _calcFileName);

        var scenarioWb = scenarioPackage.Workbook;
        var modelWb = modelPackage.Workbook;

        var correlations = GetCorrelations();

        var modelCorrelation = correlations.Models.Single(model => model.Name == _calcFileName);

        foreach (var correlation in modelCorrelation.Correlations)
        {
            var scenarioCellRange = scenarioWb.Names.Single(range => range.Name == correlation.ScenarioRange);

            var values = scenarioCellRange.GetCells()
                .Select(cell => cell.Value)
                .ToArray();

            var modelCellRange = modelWb.Names.Single(range => range.Name == correlation.ModelRange);

            try
            {
                modelCellRange.Insert(values);
            }
            catch (NamedRangeInsertException ex)
            {
                _logger.Warn("неравные именованные диапазоны", ex.Values).Write();
            }
        }

        modelWb.Calculate();
        modelPackage.Save();
    }

    private ScenarioCorrelation GetCorrelations()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var config = File.ReadAllText($"{currentDir}/{_baseFolder}/{_configFileName}");

        var result = JsonSerializer.Deserialize<ScenarioCorrelation>(config);

        return result;
    }
}
