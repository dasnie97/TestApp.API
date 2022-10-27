using Application.DTO;


namespace Application.Interfaces
{
    public interface ILogFileService
    {
        IEnumerable<LogFileDTO> GetAllLogFiles();
        LogFileDTO GetLogFileById(int id);
        LogFileDTO AddNewLogFile(CreateLogFileDTO logFile);
        void UpdateLogFile(UpdateLogFileDTO logFile);
        void DeleteLogFile(int id);
    }
}
