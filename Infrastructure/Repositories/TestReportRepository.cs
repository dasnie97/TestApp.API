using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Helpers;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using TestEngineering.Exceptions;

namespace Infrastructure.Repositories;

public class TestReportRepository : ITestReportRepository
{
    private readonly TestWatchContext _testWatchContext;
    public TestReportRepository(TestWatchContext testWatchContext)
    {
        _testWatchContext = testWatchContext;
    }

    public TestReport Add(TestReport testReport)
    {
        var relatedWorkstation = FindRelatedWorkstation(testReport);

        if (relatedWorkstation != null)
        {
            AttachToExistingWorkstation(testReport, relatedWorkstation);
            EvaluateFirstPass(testReport, relatedWorkstation);
            testReport.RecordCreated = DateTime.Now;
            _testWatchContext.TestReports.Add(testReport);
            _testWatchContext.SaveChanges();
            return testReport;
        }
        else
        {
            throw new WorkstationNotFoundException($"Workstation '{testReport.Workstation.Name}' does not exist in data base!");
        }
    }

    public TestReport Get(int id)
    {
        return _testWatchContext.TestReports.Include(t=>t.Workstation).SingleOrDefault(x => x.Id == id)!;
    }

    public void Update(TestReport testReport)
    {
        var relatedWorkstation = FindRelatedWorkstation(testReport);

        if (relatedWorkstation != null)
        {
            testReport.Workstation = relatedWorkstation;
            _testWatchContext.TestReports.Update(testReport);
            _testWatchContext.SaveChanges();
        }
        else
        {
            throw new WorkstationNotFoundException($"Workstation '{testReport.Workstation.Name}' does not exist in data base!");
        }
    }

    public void Delete(TestReport testReport)
    {
        if (testReport == null)
        {
            throw new TestReportNotFoundException("Test report does not exist!");
        }
        _testWatchContext.TestReports.Remove(testReport);
        _testWatchContext.SaveChanges();
    }

    public IEnumerable<TestReport> Get(TestReportFilter filter)
    {
        var query = _testWatchContext.
          TestReports.
          Include(t => t.Workstation).
          AsQueryable();
        query = AddFiltersOnQuery(query, filter);
        return query.OrderByDescending(x => x.RecordCreated).Take(100).
        ToList();
    }

    public IEnumerable<string> GetAllWorkstations()
    {
        return _testWatchContext.
            TestReports.
            Include(t => t.Workstation).
            AsEnumerable().
            DistinctBy(x => x.Workstation.Name).
            Select(x => x.Workstation.Name).
            OrderBy(x => x).
            ToList();
    }

    public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints(ChartInputData chartInputData)
    {
        DateTime startTime = chartInputData.DateFrom ?? DateTime.Now.AddDays(-3);
        DateTime endTime = chartInputData.DateTo ?? DateTime.Now;

        IEnumerable<IGrouping<string, TestReport>> query = BuildQuery(chartInputData, startTime, endTime);
        List<TimeInterval> timeIntervals = CalculateTimeIntervals(startTime, endTime);

        Dictionary<string, IEnumerable<YieldPoint>> yieldPoints = new();

        foreach (IGrouping<string, TestReport> workstationGroup in query)
        {
            List<YieldPoint> workstationYieldPoints = new();
            foreach (var timeInterval in timeIntervals)
            {
                var records = workstationGroup.Where(x => x.TestDateTimeStarted >= timeInterval.Start && x.TestDateTimeStarted <= timeInterval.End).ToList();
                if (!IsYieldPointOk(records))
                {
                    workstationYieldPoints.Add(new YieldPoint
                    {
                        DateAndTime = timeInterval.Start,
                        Yield = null,
                        Total = 0,
                        Passed = 0,
                        Failed = 0
                    });
                    continue;
                }
                float passed = records.Count(x => x.Status == TestStatus.Passed);
                float failed = records.Count(x => x.Status == TestStatus.Failed);
                float total = records.Count();
                workstationYieldPoints.Add(new YieldPoint
                {
                    DateAndTime = timeInterval.Start,
                    Yield = passed / total,
                    Total = (int)total,
                    Passed = (int)passed,
                    Failed = (int)failed
                });
            }
            yieldPoints.Add(workstationGroup.Key, workstationYieldPoints);
        }

        return EvaluateReturnObject(yieldPoints);
    }

    private Dictionary<string, IEnumerable<YieldPoint>> EvaluateReturnObject(Dictionary<string, IEnumerable<YieldPoint>> yieldPoints)
    {
        var sorted = yieldPoints.OrderByDescending(pair => pair.Value.Sum(val => val.Failed)).Take(5).ToDictionary(t=>t.Key, t=>t.Value);

        return sorted;
    }

    private IEnumerable<IGrouping<string, TestReport>> BuildQuery(ChartInputData chartInputData, DateTime startTime, DateTime endTime)
    {
        var query = _testWatchContext.TestReports.
            Where(x => x.TestDateTimeStarted <= endTime && x.TestDateTimeStarted >= startTime);

        if (chartInputData != null && chartInputData.Workstation.Length > 0)
        {
            query = query.Where(x => chartInputData.Workstation.Contains(x.WorkstationName));
        }

        IEnumerable<IGrouping<string, TestReport>> queryGroup = query.Where(x => x.IsFirstPass == true).AsEnumerable().GroupBy(x => x.WorkstationName);
        return queryGroup;
    }

    private List<TimeInterval> CalculateTimeIntervals(DateTime startTime, DateTime endTime)
    {
        var NUMBER_OF_INTERVALS = 24;

        List<TimeInterval> timeIntervals = new List<TimeInterval>();
        long timeSample = (endTime - startTime).Ticks / NUMBER_OF_INTERVALS;
        for (int i = 0; i < NUMBER_OF_INTERVALS; i++)
        {
            var start = new DateTime(startTime.Ticks + i * timeSample);
            var end = new DateTime(startTime.Ticks + (i + 1) * timeSample);
            timeIntervals.Add(new TimeInterval(start, end));
        }
        return timeIntervals;
    }

    private bool IsYieldPointOk(IEnumerable<TestReport> dataSet)
    {
        var passedRecords = dataSet.Where(x => x.Status == TestStatus.Passed);

        if (passedRecords.Count() == 0)
        {
            return false;
        }

        return true;
    }

    private Workstation? FindRelatedWorkstation(TestReport testReport)
    {
        var relatedWorkstation = _testWatchContext.Workstations.Where(w => w.Name == testReport.Workstation.Name).SingleOrDefault();
        return relatedWorkstation;
    }

    private void AttachToExistingWorkstation(TestReport testReport, Workstation relatedWorkstation)
    {
        testReport.Workstation = relatedWorkstation;
    }

    private void EvaluateFirstPass(TestReport testReport, Workstation relatedWorkstation)
    {
        var processStep = relatedWorkstation.ProcessStep;
        var recordsWithSameSN = _testWatchContext.TestReports.Where(t => t.SerialNumber == testReport.SerialNumber);
        var recordsWithSameSNAndProcessStep = recordsWithSameSN.Where(t => t.Workstation.ProcessStep == processStep);

        if (recordsWithSameSNAndProcessStep.Any())
        {
            testReport.IsFirstPass = false;
        }
        else
        {
            testReport.IsFirstPass = true;
        }
    }

    private IQueryable<TestReport> AddFiltersOnQuery(IQueryable<TestReport> testReports, TestReportFilter filter)
    {
        var predicate = PredicateBuilder.New<TestReport>(true); // returns all when no filters are added

        if (filter.Workstation != null && filter.Workstation.Length > 0)
        {
            var workstationPredicate = PredicateBuilder.New<TestReport>(false);
            foreach (var workstationFilter in filter.Workstation)
            {
                workstationPredicate = workstationPredicate.Or(t => t.WorkstationName.Contains(workstationFilter));
            }
            predicate = predicate.And(workstationPredicate);
        }

        if (filter.SerialNumber != null && filter.SerialNumber.Length > 0)
        {
            var serialnumberPredicate = PredicateBuilder.New<TestReport>(false);
            foreach (var serialNumberFilter in filter.SerialNumber)
            {
                serialnumberPredicate = serialnumberPredicate.Or(t=>t.SerialNumber.Contains(serialNumberFilter));
            }
            predicate = predicate.And(serialnumberPredicate);
        }

        if (filter.Dut != null && filter.Dut.Length > 0)
        {
            var dutPredicate = PredicateBuilder.New<TestReport>(false);
            foreach (var dutFilter in filter.Dut)
            {
                dutPredicate = dutPredicate.Or(t=>t.FixtureSocket.Contains(dutFilter));
            }
            predicate = predicate.And(dutPredicate);
        }

        if (filter.Failure != null && filter.Failure.Length > 0)
        {
            var failurePredicate = PredicateBuilder.New<TestReport>(false);
            foreach (var failureFilter in filter.Failure)
            {
                failurePredicate = failurePredicate.Or(t=>t.Failure.Contains(failureFilter));
            }
            predicate = predicate.And(failurePredicate);
        }

        if (filter.firstPass != null)
        {
            predicate = predicate.And(t => t.IsFirstPass == filter.firstPass);
        }

        if (filter.Result != TestStatus.Notset)
        {
            predicate = predicate.And(t=>t.Status == filter.Result);
        }

        if (filter.DateFrom != null)
        {
            predicate = predicate.And(t=>t.TestDateTimeStarted >= filter.DateFrom);
        }

        if (filter.DateTo != null)
        {
            predicate = predicate.And(t=>t.TestDateTimeStarted <= filter.DateTo);
        }

        return testReports.Where(predicate);
    }
}
