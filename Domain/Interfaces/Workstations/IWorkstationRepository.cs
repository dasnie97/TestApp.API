using Domain.Models.Workstations;

namespace Domain.Interfaces.Workstations
{
    public interface IWorkstationRepository
    {
        Workstation Add(Workstation workstation);
        IEnumerable<Workstation> Get();
        Workstation Update(Workstation workstation);
    }
}
