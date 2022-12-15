using Application.DTO.Workstations;

namespace Application.Interfaces.Workstations
{
    public interface IWorkstationService
    {
        WorkstationDTO Add(AddWorkstationDTO workstation);
        void Delete(int id);
        IEnumerable<WorkstationDTO> Get();
        WorkstationDTO Update(WorkstationDTO workstation);
    }
}
