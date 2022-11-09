using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using System.Text.Json.Serialization.Metadata;

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

        public IEnumerable<LogFile> GetAll()
        {
            return _testWatchContext.LogFiles;
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

        public IEnumerable<LogFile> GetFilteredLogFiles(string? workstation, string? serialNumber, string? result, string? dut, string? failure)
        {

            var query = _testWatchContext.
                LogFiles.
                AsEnumerable();

            if (!string.IsNullOrEmpty(workstation) && workstation != "All workstations")
            {
                query = query.Where(x => x.Workstation.ToLower().Contains(workstation.ToLower()));
            }
            if (!string.IsNullOrEmpty(serialNumber))
            {
                query = query.Where(x => x.SerialNumber.ToLower().Contains(serialNumber.ToLower()));
            }
            if (!string.IsNullOrEmpty(result))
            {
                query = query.Where(x => x.Status.ToLower().Contains(result.ToLower()));
            }
            if (!string.IsNullOrEmpty(dut))
            {
                query = query.Where(x => x.FixtureSocket.ToLower().Contains(dut.ToLower()));
            }
            if (!string.IsNullOrEmpty(failure))
            {
                query = query.Where(x => x.Failure.ToLower().Contains(failure.ToLower()));
            }

            return query.OrderByDescending(x => x.TestDateTimeStarted).
            ToList();
        }

        private bool IsYieldPointOk(IGrouping<int, LogFile> dataSet)
        {
            try
            {
                var minHourlyOutput = 1800 / dataSet.Average(x => x.TestingTime.Value.TotalSeconds);

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

        public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
        {
            var currentTime = new DateTime(2022, 10, 18);
            var query = _testWatchContext.LogFiles.AsEnumerable().
                Where(x => x.TestDateTimeStarted <= currentTime && x.TestDateTimeStarted >= currentTime.AddDays(-1))
                .GroupBy(x => x.Workstation);

            Dictionary<string, IEnumerable<YieldPoint>> yieldPoints = new Dictionary<string, IEnumerable<YieldPoint>>();

            foreach(IGrouping<string, LogFile> workstationGroup in query)
            {
                List<YieldPoint> workstationYieldPoints = new List<YieldPoint>();
                var hourGroups = workstationGroup.GroupBy(x => x.TestDateTimeStarted.Hour);
                foreach (IGrouping < int, LogFile > singleHour in hourGroups)
                {
                    if (!IsYieldPointOk(singleHour)) continue;
                    float passed = singleHour.Count(x => x.Status == "Passed");
                    float total = singleHour.Count();
                    workstationYieldPoints.Add(new YieldPoint
                    {
                        DateAndTime = currentTime.AddHours(singleHour.Key + 1),
                        Yield = passed / total
                    });
                }
                yieldPoints.Add(workstationGroup.Key, workstationYieldPoints);
            }


            return yieldPoints;
        }
    }
}
