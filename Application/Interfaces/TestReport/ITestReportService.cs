using Application.DTO.TestReport;
using Application.DTO.Workstations;
using Domain.Models;

namespace Application.Interfaces.TestReport;

public interface ITestReportService
{
    IEnumerable<TestReportDTO> GetTestReports(GetTestReportFilter? getLogFilesFilter = null);
    TestReportDTO GetTestReportByID(int id);
    TestReportDTO AddNewLogFile(CreateTestReportDTO logFile);
    void UpdateLogFile(UpdateTestReportDTO logFile);
    void DeleteLogFile(int id);
    IEnumerable<string> GetAllWorkstations();
    Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints();
}
