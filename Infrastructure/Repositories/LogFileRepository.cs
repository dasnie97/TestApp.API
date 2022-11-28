using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class LogFileRepository : ILogFileRepository
    {
        private readonly TestWatchContext _testWatchContext;
        public LogFileRepository(TestWatchContext testWatchContext)
        {
            _testWatchContext = testWatchContext;
        }

        public LogFile Add(LogFile logFile)
        {
            var isFirstPass = _testWatchContext.LogFiles.
                Where(x=>x.SerialNumber == logFile.SerialNumber && x.ProcessStep == logFile.ProcessStep).Any();
            logFile.isFirstPass = !isFirstPass;
            logFile.RecordCreated = DateTime.Now;
            _testWatchContext.LogFiles.Add(logFile);
            _testWatchContext.SaveChanges();
            return logFile;
        }

        public void Delete(LogFile logFile)
        {
            _testWatchContext.LogFiles.Remove(logFile);
            _testWatchContext.SaveChanges();
        }

        public LogFile Get(int id)
        {
            return _testWatchContext.LogFiles.SingleOrDefault(x => x.Id == id)!;
        }

        public IEnumerable<LogFile> GetAll(GetLogFilesQuery filter)
        {
            var query = _testWatchContext.
              LogFiles.
              AsQueryable();
            query = AddFiltersOnQuery(query, filter);
            return query.OrderByDescending(x => x.TestDateTimeStarted).Take(1000).
            ToList();
        }

        public void Update(LogFile logFile)
        {
            logFile.TestDateTimeStarted = DateTime.Now;
            _testWatchContext.LogFiles.Update(logFile);
            _testWatchContext.SaveChanges();
        }

        public IEnumerable<string> GetAllWorkstations()
        {         
            return _testWatchContext.
                LogFiles.
                AsEnumerable().
                DistinctBy(x => x.Workstation).
                Select(x => x.Workstation).
                OrderBy(x => x).
                ToList();
        }

        public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
        {
            var currentTime = new DateTime(2022, 11, 10, 18,0,0);
            var query = _testWatchContext.LogFiles.
                Where(x => x.TestDateTimeStarted <= currentTime && x.TestDateTimeStarted >= currentTime.AddDays(-1)).
                Where(x=>x.isFirstPass == true).AsEnumerable().GroupBy(x=>x.Workstation);

            Dictionary<string, IEnumerable<YieldPoint>> yieldPoints = new Dictionary<string, IEnumerable<YieldPoint>>();

            foreach(IGrouping<string, LogFile> workstationGroup in query)
            {
                List<YieldPoint> workstationYieldPoints = new List<YieldPoint>();
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

        private bool IsYieldPointOk(IEnumerable<LogFile> dataSet)
        {
            if (dataSet.Count() == 0)
            {
                return false;
            }
            try
            {
                var averageTestTime = dataSet.Where(x => x.Status == "Passed").Average(x => x.TestingTime.Value.TotalSeconds);
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

        private IQueryable<LogFile> AddFiltersOnQuery(IQueryable<LogFile> query, GetLogFilesQuery filters) 
        {
            query = filters.workstation.Length != 0 && filters.workstation.FirstOrDefault() != string.Empty ? query.Where(x => filters.workstation.Contains(x.Workstation)) : query;
            query = filters.serialNumber.Length != 0 && filters.serialNumber.FirstOrDefault() != string.Empty ? query.Where(x => filters.serialNumber.Contains(x.SerialNumber)) : query;
            query = filters.dut.Length != 0 ? query.Where(x => filters.dut.Contains(x.FixtureSocket)) : query;
            query = filters.failure.Length != 0 ? query.Where(x => x.Failure.Contains(filters.failure[0])) : query;
            query = filters.result != null ? query.Where(x => x.Status == filters.result) : query;
            query = filters.dateFrom != new DateTime() ? query.Where(x => x.TestDateTimeStarted >= filters.dateFrom) : query;
            query = filters.dateTo != new DateTime() ? query.Where(x => x.TestDateTimeStarted <= filters.dateTo) : query;
            return query;
        }
    }
}
