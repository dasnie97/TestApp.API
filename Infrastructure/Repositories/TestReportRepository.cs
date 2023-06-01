using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
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

    public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
    {
        var currentTime = DateTime.Now;
        var query = _testWatchContext.TestReports.
            Where(x => x.TestDateTimeStarted <= currentTime && x.TestDateTimeStarted >= currentTime.AddDays(-1)).
            Where(x => x.IsFirstPass == true).AsEnumerable().GroupBy(x => x.WorkstationName);

        Dictionary<string, IEnumerable<YieldPoint>> yieldPoints = new();

        foreach (IGrouping<string, TestReport> workstationGroup in query)
        {
            List<YieldPoint> workstationYieldPoints = new();
            foreach (var hour in Enumerable.Range(0, 25))
            {
                var time = currentTime.AddHours(-25).AddHours(hour);
                var records = workstationGroup.Where(x => x.TestDateTimeStarted.Hour == time.Hour && x.TestDateTimeStarted.Day == time.Day).ToList();
                if (!IsYieldPointOk(records))
                {
                    workstationYieldPoints.Add(new YieldPoint
                    {
                        DateAndTime = time.AddHours(1).AddMinutes(-time.Minute).AddSeconds(-time.Second),
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
                var tP = records.First().TestDateTimeStarted;
                var offset = 1;
                if (tP.Hour + 1 == 24)
                {
                    offset = -23;
                }
                workstationYieldPoints.Add(new YieldPoint
                {
                    DateAndTime = new DateTime(tP.Year, tP.Month, tP.Day, tP.Hour + offset, 0, 0),
                    Yield = passed / total,
                    Total = (int)total,
                    Passed = (int)passed,
                    Failed = (int)failed
                });
            }
            yieldPoints.Add(workstationGroup.Key, workstationYieldPoints);
        }


        return yieldPoints;
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

    private bool IsYieldPointOk(IEnumerable<TestReport> dataSet)
    {
        if (dataSet.Count() == 0)
        {
            return false;
        }

        var averageTestTime = dataSet.Where(x => x.Status == TestStatus.Passed).Average(x => x.TestingTime!.Value.TotalSeconds);

        if (averageTestTime == 0)
        {
            return true;
        }

        var minHourlyOutput = 1000 / averageTestTime;

        if (dataSet.Count() <= minHourlyOutput)
        {
            return false;
        }

        return true;
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
