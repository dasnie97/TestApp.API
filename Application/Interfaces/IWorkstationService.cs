using Application.DTO;
using TestEngineering.DTO;

namespace Application.Interfaces;

public interface IWorkstationService
{
    WorkstationDTO Add(CreateWorkstationDTO workstationDTO);
    IEnumerable<WorkstationDTO> GetAll();
    IEnumerable<WorkstationDTO> Get(WorkstationFilterDTO getWorkstationFilter);
    WorkstationDTO Update(WorkstationDTO workstation);
    void Delete(string name);
}
