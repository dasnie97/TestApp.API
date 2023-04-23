using Application.DTO;
using TestEngineering.DTO;
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
