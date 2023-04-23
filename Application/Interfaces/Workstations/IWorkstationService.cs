using Application.DTO;
using TestEngineering.DTO;

namespace Application.Interfaces.Workstations;

public interface IWorkstationService
{
    WorkstationDTO Add(CreateWorkstationDTO workstation);
    void Delete(string name);
    IEnumerable<WorkstationDTO> Get(GetWorkstationFilter? getWorkstationFilter = null);
    IEnumerable<WorkstationDTO> Get();
    WorkstationDTO Update(WorkstationDTO workstation);
}
