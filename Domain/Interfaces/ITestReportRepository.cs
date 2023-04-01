using Domain.Models;

namespace Domain.Interfaces;

public interface ITestReportRepository
{
    TestReport Add(TestReport logFile);
    void Delete(TestReport logFile);
    TestReport Get(int id);
    IEnumerable<TestReport> Get(GetLogFilesQuery getLogFilesQuery);
    void Update(TestReport logFile);
    IEnumerable<string> GetAllWorkstations();
    Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints();
}
