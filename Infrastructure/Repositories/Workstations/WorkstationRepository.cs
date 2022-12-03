using Domain.Interfaces.Workstations;
using Domain.Models.Workstations;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Workstations
{
    public class WorkstationRepository : IWorkstationRepository
    {
        private readonly TestWatchContext _testWatchContext;
        public WorkstationRepository(TestWatchContext testWatchContext)
        {
            _testWatchContext = testWatchContext;
        }

        public Workstation Add(Workstation workstation)
        {
            workstation.RecordCreated = DateTime.Now;
            _testWatchContext.Workstations.Add(workstation);
            _testWatchContext.SaveChanges();
            return workstation;
        }

        public IEnumerable<Workstation> Get()
        {
            return _testWatchContext.Workstations;
        }

        public Workstation Update(Workstation workstation)
        {
            workstation.RecordUpdated = DateTime.Now;
            _testWatchContext.Workstations.Update(workstation);
            _testWatchContext.SaveChanges();
            return workstation;
        }
    }
}
