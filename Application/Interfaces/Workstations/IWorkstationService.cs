using ProductTest.DTO;

namespace Application.Interfaces.Workstations;

public interface IWorkstationService
{
    WorkstationDTO Add(CreateWorkstationDTO workstation);
    void Delete(int id);
    IEnumerable<WorkstationDTO> Get();
    WorkstationDTO Update(WorkstationDTO workstation);
}
