using Application.DTO;
using TestEngineering.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Application.Interfaces;
using TestEngineering.Exceptions;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DowntimeReportController : ControllerBase
{
    private readonly IDowntimeReportService _downtimeReportService;
    private readonly ILogger<DowntimeReportController> _logger;

    public DowntimeReportController(IDowntimeReportService downtimeReportService, ILogger<DowntimeReportController> logger)
    {
        _downtimeReportService = downtimeReportService;
        _logger = logger;
    }


    [SwaggerOperation(Summary = "Creates new downtime report")]
    [HttpPost]
    public IActionResult Create(CreateDowntimeReportDTO newDowntimeReport)
    {
        try
        {
            var downtimeReport = _downtimeReportService.Add(newDowntimeReport);
            return Created($"api/downtimereport/{downtimeReport.Id}", downtimeReport);
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

    [SwaggerOperation(Summary = "Gets downtime report by id")]
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var downtimeReport = _downtimeReportService.GetDowntimeReportByID(id);
        if (downtimeReport == null) return NotFound();
        return Ok(downtimeReport);
    }

    [SwaggerOperation(Summary = "Updates downtime report")]
    [HttpPut]
    public IActionResult Update(UpdateDowntimeReportDTO updatedDowntimeReport)
    {
        try
        {
            _downtimeReportService.Update(updatedDowntimeReport);
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

    [SwaggerOperation(Summary = "Deletes downtime report")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        try
        {
            _downtimeReportService.Delete(id);
            return NoContent();
        }
        catch (DowntimeReportNotFoundException ex)
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

    [SwaggerOperation(Summary = "Gets downtime reports according to filter")]
    [HttpGet]
    public IActionResult GetFiltered([FromQuery] DowntimeReportFilterDTO downtimeReportFilter)
    {
        var downtimeReports = _downtimeReportService.GetDowntimeReports(downtimeReportFilter);
        return Ok(downtimeReports);
    }
}
