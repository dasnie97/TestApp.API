using Domain.Models;

namespace Domain.Interfaces;

public interface IWorkstationRepository
{
    Workstation Add(Workstation workstation);
    void Delete(Workstation workstation);
    IEnumerable<Workstation> Get(GetWorkstationsQuery filter);
    IEnumerable<Workstation> Get();
    Workstation Update(Workstation workstation);
}
