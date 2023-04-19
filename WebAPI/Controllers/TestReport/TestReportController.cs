using Application.DTO;
using ProductTest.DTO;
using Application.Interfaces.TestReport;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.LogFiles
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestReportController : ControllerBase
    {
        private readonly ITestReportService _logFileService;
        public TestReportController(ITestReportService logFileService)
        {
            _logFileService = logFileService;
        }

        [SwaggerOperation(Summary = "Gets test reports according to filter values")]
        [HttpGet]
        public IActionResult GetFiltered([FromQuery] GetTestReportFilter logFileFilterParameters)
        {
            var filteredLogFiles = _logFileService.GetTestReports(logFileFilterParameters);
            return Ok(filteredLogFiles);
        }

        [SwaggerOperation(Summary = "Gets test report by id")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var logFile = _logFileService.GetTestReportByID(id);
            if (logFile == null) return NotFound();
            return Ok(logFile);
        }

        [SwaggerOperation(Summary = "Creates new test report")]
        [HttpPost]
        public IActionResult Create(CreateTestReportDTO newLogFile)
        {
            var logFile = _logFileService.AddNewLogFile(newLogFile);
            return Created($"api/logfiles/{logFile.Id}", logFile);
        }

        [SwaggerOperation(Summary = "Updates existing log file")]
        [HttpPut]
        public IActionResult Update(UpdateTestReportDTO updateLogFile)
        {
            _logFileService.UpdateLogFile(updateLogFile);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Deletes log file")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _logFileService.DeleteLogFile(id);
            return NoContent();
        }

        [SwaggerOperation(Summary = "Gets all unique workstations")]
        [HttpGet]
        [Route("workstations")]
        public IActionResult GetAllWorkstations()
        {
            var workstations = _logFileService.GetAllWorkstations();
            return Ok(workstations);
        }

        [SwaggerOperation(Summary = "Gets yield of each workstation from last 24 hours")]
        [HttpGet("yield")]
        public IActionResult GetYieldPoints()
        {
            var yieldPoints = _logFileService.GetYieldPoints();
            return Ok(yieldPoints);
        }
    }
}
