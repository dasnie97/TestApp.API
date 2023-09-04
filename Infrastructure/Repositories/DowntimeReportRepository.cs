using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using TestEngineering.Exceptions;

namespace Infrastructure.Repositories;

public class DowntimeReportRepository : IDowntimeReportRepository
{
    private readonly TestWatchContext _testWatchContext;
    public DowntimeReportRepository(TestWatchContext testWatchContext)
    {
        _testWatchContext = testWatchContext;
    }

    public DowntimeReport Add(DowntimeReport downtimeReport)
    {
        var relatedWorkstation = FindRelatedWorkstation(downtimeReport);

        if (relatedWorkstation != null)
        {
            AttachToExistingWorkstation(downtimeReport, relatedWorkstation);
            downtimeReport.RecordCreated = DateTime.Now;
            _testWatchContext.DowntimeReports.Add(downtimeReport);
            _testWatchContext.SaveChanges();
            return downtimeReport;
        }
        else
        {
            throw new WorkstationNotFoundException($"Workstation '{downtimeReport.Workstation.Name}' does not exist in data base!");
        }
    }

    public DowntimeReport Get(int id)
    {
        return _testWatchContext.DowntimeReports.Include(t=>t.Workstation).SingleOrDefault(x => x.Id == id)!;
    }

    public void Update(DowntimeReport downtimeReport)
    {
        var relatedWorkstation = FindRelatedWorkstation(downtimeReport);

        if (relatedWorkstation != null)
        {
            downtimeReport.Workstation = relatedWorkstation;
            _testWatchContext.DowntimeReports.Update(downtimeReport);
            _testWatchContext.SaveChanges();
        }
        else
        {
            throw new WorkstationNotFoundException($"Workstation '{downtimeReport.Workstation.Name}' does not exist in data base!");
        }
    }

    public void Delete(DowntimeReport downtimeReport)
    {
        if (downtimeReport == null)
        {
            throw new DowntimeReportNotFoundException("Downtime report does not exist!");
        }
        _testWatchContext.DowntimeReports.Remove(downtimeReport);
        _testWatchContext.SaveChanges();
    }

    public IEnumerable<DowntimeReport> Get(DowntimeReportFilter filter)
    {
        var query = _testWatchContext.
          DowntimeReports.
          Include(t => t.Workstation).
          AsQueryable();
        query = AddFiltersOnQuery(query, filter);
        return query.OrderByDescending(x => x.RecordCreated).Take(100).
        ToList();
    }

    private Workstation? FindRelatedWorkstation(DowntimeReport downtimeReport)
    {
        var relatedWorkstation = _testWatchContext.Workstations.Where(w => w.Name == downtimeReport.Workstation.Name).SingleOrDefault();
        return relatedWorkstation;
    }

    private void AttachToExistingWorkstation(DowntimeReport downtimeReport, Workstation relatedWorkstation)
    {
        downtimeReport.Workstation = relatedWorkstation;
    }

    private IQueryable<DowntimeReport> AddFiltersOnQuery(IQueryable<DowntimeReport> downtimeReports, DowntimeReportFilter filter)
    {
        var predicate = PredicateBuilder.New<DowntimeReport>(true); // returns all when no filters are added

        if (filter.ProblemDescription != null && filter.ProblemDescription.Length > 0)
        {
            var problemDescriptionPredicate = PredicateBuilder.New<DowntimeReport>(false);
            foreach (var problemDescriptionFilter in filter.ProblemDescription)
            {
                problemDescriptionPredicate = problemDescriptionPredicate.Or(t => t.ProblemDescription.Contains(problemDescriptionFilter));
            }
            predicate = predicate.And(problemDescriptionPredicate);
        }

        if (filter.ActionTaken != null && filter.ActionTaken.Length > 0)
        {
            var actionTakenPredicate = PredicateBuilder.New<DowntimeReport>(false);
            foreach (var actionTakenFilter in filter.ActionTaken)
            {
                actionTakenPredicate = actionTakenPredicate.Or(t => t.ActionTaken.Contains(actionTakenFilter));
            }
            predicate = predicate.And(actionTakenPredicate);
        }

        if (filter.Technician != null && filter.Technician.Length > 0)
        {
            var technicianPredicate = PredicateBuilder.New<DowntimeReport>(false);
            foreach (var technicianFilter in filter.Technician)
            {
                technicianPredicate = technicianPredicate.Or(t => t.Technician.Contains(technicianFilter));
            }
            predicate = predicate.And(technicianPredicate);
        }

        if (filter.Workstation != null && filter.Workstation.Length > 0)
        {
            var workstationPredicate = PredicateBuilder.New<DowntimeReport>(false);
            foreach (var workstationFilter in filter.Workstation)
            {
                workstationPredicate = workstationPredicate.Or(t => t.WorkstationName.Contains(workstationFilter));
            }
            predicate = predicate.And(workstationPredicate);
        }

        if (filter.Operator != null && filter.Operator.Length > 0)
        {
            var operatorPredicate = PredicateBuilder.New<DowntimeReport>(false);
            foreach (var operatorFilter in filter.Operator)
            {
                operatorPredicate = operatorPredicate.Or(t => t.Operator.Contains(operatorFilter));
            }
            predicate = predicate.And(operatorPredicate);
        }

        if (filter.DateFrom != null)
        {
            predicate = predicate.And(t=>t.TimeStarted >= filter.DateFrom);
        }

        if (filter.DateTo != null)
        {
            predicate = predicate.And(t=>t.TimeFinished <= filter.DateTo);
        }

        return downtimeReports.Where(predicate);
    }
}
