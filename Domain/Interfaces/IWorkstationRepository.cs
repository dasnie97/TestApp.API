using Domain.Models;

namespace Domain.Interfaces;

public interface IWorkstationRepository
{
    Workstation Add(Workstation workstation);
    IEnumerable<Workstation> Get(WorkstationFilter filter);
    Workstation Update(Workstation workstation);
    void Delete(Workstation workstation);
}
