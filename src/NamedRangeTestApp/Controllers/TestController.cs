using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.Models;
using NamedRangeTestApp.Services.Base;
using System;

namespace NamedRangeTestApp.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly INamedRangeService _namedRangeService;
    private readonly ITestExcelService _testExcelService;

    public TestController(
        ILogger<TestController> logger,
        INamedRangeService namedRangeService,
        ITestExcelService testExcelService
        )
    {
        _logger = logger;
        _namedRangeService = namedRangeService;
        _testExcelService = testExcelService;
    }

    /// <summary>
    /// Inserting data from input model into the scenario file
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("scenario")]
    public IActionResult PostScenario([FromBody] ScenarioInputModel input)
    {
        _testExcelService.AddValuesToScenario(input.NamedRanges);

        return StatusCode(201);
    }

    /// <summary>
    /// Recalculation of test_calc file using correlations in config file
    /// </summary>
    /// <returns></returns>
    [HttpPost("calc")]
    public IActionResult CalcModel()
    {
        _testExcelService.RecalculateModel();

        return StatusCode(201);
    }

    /// <summary>
    /// Get cells by named range name
    /// </summary>
    /// <param name="namedRange"></param>
    /// <returns></returns>
    [HttpGet("by-name")]
    public IActionResult GetByName(string inputNamedRange)
    {
        var q = Enum.Parse<CalcNamedRange>(inputNamedRange);

        var result = _namedRangeService.GetRangeData(q);

        return Ok(result);
    }

    /// <summary>
    /// Get cells by named range number
    /// </summary>
    /// <param name="namedRange"></param>
    /// <returns></returns>
    [HttpGet("by-number")]
    public IActionResult GetByNumber(CalcNamedRange namedRange)
    {
        var result = _namedRangeService.GetRangeData(namedRange);

        return Ok(result);
    }

    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            NamedRanges = new object[]
            {
                new { Name = "Простые операции", NamedRange = "simple_operations", NamedRangeNum = 0},
                new { Name = "Логические", NamedRange = "logical_operations", NamedRangeNum = 1},
                new { Name = "Математические", NamedRange = "math_operations", NamedRangeNum = 2},
                new { Name = "Прочее", NamedRange = "other", NamedRangeNum = 3},
                new { Name = "Функция массива (вертикальная)", NamedRange = "column_cells_sum", NamedRangeNum = 4},
                new { Name = "Функция массива (горизонтальная)", NamedRange = "row_cells_sum", NamedRangeNum = 5},
            }
        });
    }
}

