using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NamedRangeTestApp.DataAccess.Base;

namespace NamedRangeTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IExcelService _excelService;

        public TestController(ILogger<TestController> logger, IExcelService excelService)
        {
            _logger = logger;
            _excelService = excelService;
        }

        [HttpGet]
        public IActionResult Get(string namedRange = "TestRange")
        {
            var result = _excelService.GetCellsByNamedRange(namedRange);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return StatusCode(201);
        }
    }
}
