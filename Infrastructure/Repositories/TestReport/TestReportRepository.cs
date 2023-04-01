using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories.LogFiles
{
    public class TestReportRepository : ITestReportRepository
    {
        private readonly TestWatchContext _testWatchContext;
        public TestReportRepository(TestWatchContext testWatchContext)
        {
            _testWatchContext = testWatchContext;
        }

        public TestReport Add(TestReport logFile)
        {
            var isFirstPass = _testWatchContext.TestReports.
                Where(x => x.SerialNumber == logFile.SerialNumber && x.ProcessStep == logFile.ProcessStep).Any();
            logFile.IsFirstPass = !isFirstPass;
            logFile.RecordCreated = DateTime.Now;

            var workstationDoesntExists = !_testWatchContext.Workstations.Where(w => w.Name == logFile.Workstation).Any();
            if (workstationDoesntExists)
            {
                _testWatchContext.Workstations.Add(new Workstation(logFile.Workstation));
            }

            _testWatchContext.TestReports.Add(logFile);
            _testWatchContext.SaveChanges();
            return logFile;
        }

        public void Delete(TestReport logFile)
        {
            _testWatchContext.TestReports.Remove(logFile);
            _testWatchContext.SaveChanges();
        }

        public TestReport Get(int id)
        {
            return _testWatchContext.TestReports.SingleOrDefault(x => x.Id == id)!;
        }

        public IEnumerable<TestReport> Get(GetLogFilesQuery filter = null)
        {
            var query = _testWatchContext.
              TestReports.
              AsQueryable();
            query = AddFiltersOnQuery(query, filter);
            return query.OrderByDescending(x => x.TestDateTimeStarted).Take(1000).
            ToList();
        }

        public void Update(TestReport logFile)
        {
            _testWatchContext.TestReports.Update(logFile);
            _testWatchContext.SaveChanges();
        }

        public IEnumerable<string> GetAllWorkstations()
        {
            return _testWatchContext.
                TestReports.
                AsEnumerable().
                DistinctBy(x => x.Workstation).
                Select(x => x.Workstation).
                OrderBy(x => x).
                ToList();
        }

        public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
        {
            var currentTime = new DateTime(2022, 11, 10, 18, 0, 0);
            var query = _testWatchContext.TestReports.
                Where(x => x.TestDateTimeStarted <= currentTime && x.TestDateTimeStarted >= currentTime.AddDays(-1)).
                Where(x => x.IsFirstPass == true).AsEnumerable().GroupBy(x => x.Workstation);

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
                    float passed = records.Count(x => x.Status == "Passed");
                    float failed = records.Count(x => x.Status == "Failed");
                    float total = records.Count();
                    var tP = records.First().TestDateTimeStarted;
                    workstationYieldPoints.Add(new YieldPoint
                    {
                        DateAndTime = new DateTime(tP.Year, tP.Month, tP.Day, tP.Hour + 1, 0, 0),
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

        private bool IsYieldPointOk(IEnumerable<TestReport> dataSet)
        {
            if (dataSet.Count() == 0)
            {
                return false;
            }
            try
            {
                var averageTestTime = dataSet.Where(x => x.Status == "Passed").Average(x => x.TestingTime!.Value.TotalSeconds);
                var minHourlyOutput = 1000 / averageTestTime;

                if (averageTestTime == 0)
                {
                    throw new Exception("Average test time is 0!");
                }

                if (dataSet.Count() <= minHourlyOutput)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }

            return true;
        }

        private IQueryable<TestReport> AddFiltersOnQuery(IQueryable<TestReport> query, GetLogFilesQuery filters)
        {
            query = filters.Workstation?.FirstOrDefault() != null && filters.Workstation.Length != 0 ? query.Where(x => filters.Workstation.Contains(x.Workstation)) : query;
            query = filters.firstPass != null ? query.Where(x => x.IsFirstPass == filters.firstPass) : query;
            query = filters.SerialNumber?.FirstOrDefault() != null && filters.SerialNumber.Length != 0 ? query.Where(x => filters.SerialNumber.Contains(x.SerialNumber)) : query;
            query = filters.Dut?.FirstOrDefault() != null && filters.Dut.Length != 0 ? query.Where(x => filters.Dut.Contains(x.FixtureSocket)) : query;
            query = filters.Failure?.FirstOrDefault() != null && filters.Failure.Length != 0 ? query.Where(x => x.Failure.Contains(filters.Failure[0])) : query;
            query = filters.Result != null ? query.Where(x => x.Status == filters.Result) : query;
            query = filters.DateFrom != new DateTime() ? query.Where(x => x.TestDateTimeStarted >= filters.DateFrom) : query;
            query = filters.DateTo != new DateTime() ? query.Where(x => x.TestDateTimeStarted <= filters.DateTo) : query;
            return query;
        }
    }
}
