using Domain.Models.LogFiles;

namespace Domain.Interfaces.LogFiles
{
    public interface ILogFileRepository
    {
        LogFile Add(LogFile logFile);
        void Delete(LogFile logFile);
        LogFile Get(int id);
        IEnumerable<LogFile> GetAll(GetLogFilesQuery getLogFilesQuery);
        void Update(LogFile logFile);
        IEnumerable<string> GetAllWorkstations();
        Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints();
    }
}
