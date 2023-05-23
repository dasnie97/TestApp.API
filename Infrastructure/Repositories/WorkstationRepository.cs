using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

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

    public IEnumerable<Workstation> Get(WorkstationFilter filter)
    {
        var query = _testWatchContext.Workstations.AsNoTracking().AsQueryable();
        query = AddFiltersOnQuery(query, filter);
        return query.OrderBy(w => w.Name).ToList();
    }

    public Workstation Update(Workstation workstation)
    {
        _testWatchContext.Workstations.Update(workstation);
        _testWatchContext.SaveChanges();
        return workstation;
    }

    public void Delete(Workstation workstation)
    {
        _testWatchContext.Workstations.Remove(workstation);
        _testWatchContext.SaveChanges();
    }

    private IQueryable<Workstation> AddFiltersOnQuery(IQueryable<Workstation> workstations, WorkstationFilter filter)
    {
        var predicate = PredicateBuilder.New<Workstation>(true); // returns all if no filters are added

        if (filter.Name != null && filter.Name.Length > 0)
        {
            foreach (var workstationFilter in filter.Name)
            {
                predicate = predicate.Or(w => w.Name.Contains(workstationFilter));
            }
        }

        return workstations.Where(predicate);
    }
}
