using Application.DTO;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ILogFileService
    {
        IEnumerable<LogFileDTO> GetAllLogFiles(GetLogFilesFilter? getLogFilesFilter = null);
        LogFileDTO GetLogFileById(int id);
        LogFileDTO AddNewLogFile(CreateLogFileDTO logFile);
        void UpdateLogFile(UpdateLogFileDTO logFile);
        void DeleteLogFile(int id);
        IEnumerable<string> GetAllWorkstations();
        Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints();
    }
}
