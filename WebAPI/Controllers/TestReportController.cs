﻿using Application.DTO;
using TestEngineering.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Application.Interfaces;
using TestEngineering.Exceptions;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestReportController : ControllerBase
{
    private readonly ITestReportService _testReportService;
    private readonly ILogger<TestReportController> _logger;

    public TestReportController(ITestReportService testReportService, ILogger<TestReportController> logger)
    {
        _testReportService = testReportService;
        _logger = logger;
    }


    [SwaggerOperation(Summary = "Creates new test report")]
    [HttpPost]
    public IActionResult Create(CreateTestReportDTO newTestReport)
    {
        try
        {
            var testReport = _testReportService.Add(newTestReport);
            return Created($"api/testreport/{testReport.Id}", testReport);
        }
        catch (WorkstationNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [SwaggerOperation(Summary = "Gets test report by id")]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var testreport = _testReportService.GetTestReportByID(id);
        if (testreport == null) return NotFound();
        return Ok(testreport);
    }

    [SwaggerOperation(Summary = "Updates test report")]
    [HttpPut]
    public IActionResult Update(UpdateTestReportDTO updatedTestReport)
    {
        try
        {
            _testReportService.Update(updatedTestReport);
            return NoContent();
        }
        catch (WorkstationNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [SwaggerOperation(Summary = "Deletes test report")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        try
        {
            _testReportService.Delete(id);
            return NoContent();
        }
        catch (TestReportNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [SwaggerOperation(Summary = "Gets test reports according to filter")]
    [HttpGet]
    public IActionResult GetFiltered([FromQuery] TestReportFilterDTO testReportFilter)
    {
        var testReports = _testReportService.GetTestReports(testReportFilter);
        return Ok(testReports);
    }

    [SwaggerOperation(Summary = "Gets all unique workstations")]
    [HttpGet]
    [Route("workstations")]
    public IActionResult GetAllWorkstations()
    {
        var workstations = _testReportService.GetAllWorkstations();
        return Ok(workstations);
    }

    [SwaggerOperation(Summary = "Gets yield of each workstation from last 24 hours")]
    [HttpGet("yield")]
    public IActionResult GetYieldPoints([FromQuery] ChartInputDataDTO chartInputData)
    {
        var yieldPoints = _testReportService.GetYieldPoints(chartInputData);
        return Ok(yieldPoints);
    }
}
