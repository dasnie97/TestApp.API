using Application.DTO.TestReport;
using Application.DTO.Workstations;
using Application.Interfaces.TestReport;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services.LogFiles
{
    public class TestReportService : ITestReportService
    {
        private readonly ITestReportRepository _logFileRepository;
        private readonly IMapper _mapper;
        public TestReportService(ITestReportRepository logFileRepository, IMapper mapper)
        {
            _logFileRepository = logFileRepository;
            _mapper = mapper;
        }

        public TestReportDTO AddNewLogFile(CreateTestReportDTO logFile)
        {
            if (string.IsNullOrEmpty(logFile.SerialNumber)) throw new Exception("Log file has to have serial number!");
            var mappedLogFile = _mapper.Map<TestReport>(logFile);
            _logFileRepository.Add(mappedLogFile);
            return _mapper.Map<TestReportDTO>(mappedLogFile);
        }

        public void DeleteLogFile(int id)
        {
            var logFile = _logFileRepository.Get(id);
            if (logFile == null)
                throw new Exception("Resource does not exist.");
            _logFileRepository.Delete(logFile);
        }

        public IEnumerable<TestReportDTO> GetTestReports(GetTestReportFilter? getLogFilesFilter = null)
        {
            var filter = _mapper.Map<GetLogFilesQuery>(getLogFilesFilter);
            var filteredLogFiles = _logFileRepository.Get(filter);
            return _mapper.Map<IEnumerable<TestReportDTO>>(filteredLogFiles);
        }

        public TestReportDTO GetTestReportByID(int id)
        {
            var logFile = _logFileRepository.Get(id);
            return _mapper.Map<TestReportDTO>(logFile);
        }

        public void UpdateLogFile(UpdateTestReportDTO updateLogFile)
        {
            var originalLogFile = _logFileRepository.Get(updateLogFile.Id);
            var logFile = _mapper.Map(updateLogFile, originalLogFile);
            _logFileRepository.Update(logFile);
        }

        public IEnumerable<string> GetAllWorkstations()
        {
            return _mapper.Map<IEnumerable<string>>(_logFileRepository.GetAllWorkstations());
        }

        public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
        {
            return _logFileRepository.GetYieldPoints();
        }
    }
}
