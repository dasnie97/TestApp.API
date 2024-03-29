﻿using TestEngineering.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Application.DTO;
using Application.Interfaces;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkstationController : ControllerBase
{
    private readonly IWorkstationService _workstationService;
    private readonly ILogger<WorkstationController> _logger;

    public WorkstationController(IWorkstationService workstationService, ILogger<WorkstationController> logger)
    {
        _workstationService = workstationService;
        _logger = logger;
    }


    [SwaggerOperation(Summary = "Create new workstation")]
    [HttpPost]
    public IActionResult Add(CreateWorkstationDTO newWorkstation)
    {
        var workstation = _workstationService.Add(newWorkstation);
        return Created($"api/workstation/{workstation.Name}", workstation);
    }

    [SwaggerOperation(Summary = "Gets workstations according to filter values")]
    [HttpGet]
    public IActionResult GetFiltered([FromQuery] WorkstationFilterDTO workstationFilterParameters)
    {
        var filteredWorkstations = _workstationService.Get(workstationFilterParameters);
        return Ok(filteredWorkstations);
    }

    [SwaggerOperation(Summary = "Update workstation")]
    [HttpPut]
    public IActionResult UpdateWorkstation(WorkstationDTO workstation)
    {
        var updated = _workstationService.Update(workstation);
        return Ok(updated);
    }

    [SwaggerOperation(Summary = "Delete workstation")]
    [HttpDelete]
    public IActionResult DeleteWorkstation(string name)
    {
        _workstationService.Delete(name);
        return NoContent();
    }
}
