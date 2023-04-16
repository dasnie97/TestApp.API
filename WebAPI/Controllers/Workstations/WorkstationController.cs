using ProductTest.DTO;
using Application.Interfaces.Workstations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Application.DTO;

namespace WebAPI.Controllers.Workstations
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkstationController : ControllerBase
    {
        private readonly IWorkstationService _workstationService;
        public WorkstationController(IWorkstationService workstationService)
        {
            _workstationService= workstationService;
        }

        [SwaggerOperation(Summary ="Create new workstation")]
        [HttpPost]
        public IActionResult AddNewWorkstation(CreateWorkstationDTO newWorkstation)
        {
            var workstation = _workstationService.Add(newWorkstation);
            return Created($"api/workstation/{workstation.Id}", workstation);
        }

        [SwaggerOperation(Summary = "Gets workstations according to filter values")]
        [HttpGet]
        public IActionResult GetFiltered([FromQuery] GetWorkstationFilter workstationFilterParameters)
        {
            var filteredWorkstations = _workstationService.Get(workstationFilterParameters);
            return Ok(filteredWorkstations);
        }

        //[SwaggerOperation(Summary = "Get workstations")]
        //[HttpGet]
        //public IActionResult GetWorkstations()
        //{
        //    var workstations = _workstationService.Get();
        //    return Ok(workstations);
        //}

        [SwaggerOperation(Summary ="Update workstation")]
        [HttpPut]
        public IActionResult UpdateWorkstation(WorkstationDTO workstation)
        {
            var updated = _workstationService.Update(workstation);
            return Ok(updated);
        }

        [SwaggerOperation(Summary ="Delete workstation")]
        [HttpDelete]
        public IActionResult DeleteWorkstation(int id)
        {
            _workstationService.Delete(id);
            return NoContent();
        }
    }
}
