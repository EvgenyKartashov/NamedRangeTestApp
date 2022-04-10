using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.Models;

namespace NamedRangeTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly INamedRangeExcelService _namedRangeExcelService;
        private readonly ITestExcelService _testExcelService;

        public TestController(ILogger<TestController> logger, INamedRangeExcelService excelService, ITestExcelService testExcelService)
        {
            _logger = logger;
            _namedRangeExcelService = excelService;
            _testExcelService = testExcelService;
        }

        [HttpGet]
        public IActionResult Get(string namedRange = "TestRange")
        {
            var result = _namedRangeExcelService.GetCellsByNamedRange(namedRange);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] InputModel input)
        {
            _namedRangeExcelService.InsertValuesToNamedRange(input.NamedRange, input.Values);

            return StatusCode(201);
        }

        [HttpPost("scenario")]
        public IActionResult PostScenario([FromBody] InputModel input)
        {
            var result = _testExcelService.CheckNamedRangeReferences(input.NamedRange, input.Values);

            return StatusCode(201, result);
        }
    }
}
