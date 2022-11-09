﻿using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogFileController : ControllerBase
    {
        private readonly ILogFileService _logFileService;
        public LogFileController(ILogFileService logFileService)
        {
            _logFileService = logFileService;
        }

        [SwaggerOperation(Summary ="Retrieves all log files")]
        [HttpGet]
        public IActionResult Get()
        {
            var logFiles = _logFileService.GetAllLogFiles();
            return Ok(logFiles);
        }

        [SwaggerOperation(Summary = "Retrieves log file by id")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var logFile = _logFileService.GetLogFileById(id);
            if (logFile == null) return NotFound();
            return Ok(logFile);
        }

        [SwaggerOperation(Summary ="Create new log file")]
        [HttpPost]
        public IActionResult Create(CreateLogFileDTO newLogFile)
        {
            var logFile = _logFileService.AddNewLogFile(newLogFile);
            return Created($"api/logfiles/{logFile.Id}", logFile);
        }

        [SwaggerOperation(Summary ="Updates existing log file")]
        [HttpPut]
        public IActionResult Update(UpdateLogFileDTO updateLogFile)
        {
            _logFileService.UpdateLogFile(updateLogFile);
            return NoContent();
        }

        [SwaggerOperation(Summary ="Deletes log file")]
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

        [SwaggerOperation(Summary ="Gets Log Files according to filter values")]
        [HttpGet("filter")]
        public IActionResult GetFilteredLogFiles(string? workstation=null, string? serialNumber = null, string? result = null, string? dut = null, string? failure = null)
        {
            var filteredLogFiles = _logFileService.GetFilteredLogFiles(workstation, serialNumber, result, dut, failure);
            return Ok(filteredLogFiles);
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
