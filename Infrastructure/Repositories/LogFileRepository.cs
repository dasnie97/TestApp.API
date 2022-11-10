using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using System.Diagnostics.Contracts;
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

        /// <summary>
        /// Checks if input data came from regular production - returns true if so
        /// </summary>
        /// <param name="dataSet">Input data set</param>
        /// <returns>True if data is considered to be from regular production. Otherwise false.</returns>
        private bool IsYieldPointOk(IGrouping<int, LogFile> dataSet)
        {
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

        public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
        {
            var currentTime = new DateTime(2022, 11, 10, 18,0,0);
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
                    float failed = singleHour.Count(x => x.Status == "Failed");
                    float total = singleHour.Count();
                    var tP = singleHour.First().TestDateTimeStarted;
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
    }
}
