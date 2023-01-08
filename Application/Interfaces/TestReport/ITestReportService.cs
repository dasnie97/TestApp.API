using Application.DTO.TestReport;
using Application.DTO.Workstations;
using Domain.Models;

namespace Application.Interfaces.TestReport;

public interface ITestReportService
{
    IEnumerable<TestReportDTO> GetAllLogFiles(GetTestReportFilter? getLogFilesFilter = null);
    TestReportDTO GetLogFileById(int id);
    TestReportDTO AddNewLogFile(CreateTestReportDTO logFile);
    void UpdateLogFile(UpdateTestReportDTO logFile);
    void DeleteLogFile(int id);
    IEnumerable<WorkstationDTO> GetAllWorkstations();
    Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints();
}
