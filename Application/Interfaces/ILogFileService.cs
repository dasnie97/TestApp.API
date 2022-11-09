using Application.DTO;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ILogFileService
    {
        IEnumerable<LogFileDTO> GetAllLogFiles();
        LogFileDTO GetLogFileById(int id);
        LogFileDTO AddNewLogFile(CreateLogFileDTO logFile);
        void UpdateLogFile(UpdateLogFileDTO logFile);
        void DeleteLogFile(int id);
        IEnumerable<string> GetAllWorkstations();
        IEnumerable<LogFileDTO> GetFilteredLogFiles(string? workstation, string? serialNumber, string? result, string? dut, string? failure);
        Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints();
    }
}
