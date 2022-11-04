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
    }
}
