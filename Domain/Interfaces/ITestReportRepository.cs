using Domain.Models;

namespace Domain.Interfaces;

public interface ITestReportRepository
{
    TestReport Add(TestReport testReport);
    TestReport Get(int id);
    void Update(TestReport testReport);
    void Delete(TestReport testReport);
    IEnumerable<TestReport> Get(TestReportFilter testReportFilter);
    IEnumerable<string> GetAllWorkstations();
    Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints(ChartInputData chartInputData);
}
