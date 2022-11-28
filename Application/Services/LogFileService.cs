using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class LogFileService : ILogFileService
    {
        private readonly ILogFileRepository _logFileRepository;
        private readonly IMapper _mapper;
        public LogFileService(ILogFileRepository logFileRepository, IMapper mapper)
        {
            _logFileRepository = logFileRepository;
            _mapper = mapper;
        }

        public LogFileDTO AddNewLogFile(CreateLogFileDTO logFile)
        {
            if (string.IsNullOrEmpty(logFile.SerialNumber)) throw new Exception("Log file has to have serial number!");
            var mappedLogFile = _mapper.Map<LogFile>(logFile);
            _logFileRepository.Add(mappedLogFile);
            return _mapper.Map<LogFileDTO>(mappedLogFile);
        }

        public void DeleteLogFile(int id)
        {
            var logFile = _logFileRepository.Get(id);
            if (logFile == null)
                throw new Exception("Resource does not exist.");
            _logFileRepository.Delete(logFile);
        }

        public IEnumerable<LogFileDTO> GetAllLogFiles(GetLogFilesFilter? getLogFilesFilter = null)
        {
            var filter = _mapper.Map<GetLogFilesQuery>(getLogFilesFilter);
            var filteredLogFiles = _logFileRepository.GetAll(filter);
            return _mapper.Map<IEnumerable<LogFileDTO>>(filteredLogFiles);
        }

        public LogFileDTO GetLogFileById(int id)
        {
            var logFile = _logFileRepository.Get(id);
            return _mapper.Map<LogFileDTO>(logFile);
        }

        public void UpdateLogFile(UpdateLogFileDTO updateLogFile)
        {
            var originalLogFile = _logFileRepository.Get(updateLogFile.Id);
            var logFile = _mapper.Map(updateLogFile, originalLogFile);
            _logFileRepository.Update(logFile);
        }

        public IEnumerable<string> GetAllWorkstations()
        {
            return _logFileRepository.GetAllWorkstations();
        }

        public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
        {
            return _logFileRepository.GetYieldPoints();
        }
    }
}
