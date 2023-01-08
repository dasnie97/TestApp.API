using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public void Delete(Workstation workstation)
        {
            _testWatchContext.Workstations.Remove(workstation);
            _testWatchContext.SaveChanges();
        }

        public IEnumerable<Workstation> Get()
        {
            return _testWatchContext.Workstations.AsNoTracking();
        }

        public Workstation Update(Workstation workstation)
        {
            _testWatchContext.Workstations.Update(workstation);
            _testWatchContext.SaveChanges();
            return workstation;
        }
    }
}
