using Application.DTO;
using TestEngineering.DTO;
using Domain.Models;

namespace Application.Interfaces;

public interface ITestReportService
{
    TestReportDTO Add(CreateTestReportDTO testReportDTO);
    TestReportDTO GetTestReportByID(int id);
    void Update(UpdateTestReportDTO testReportDTO);
    void Delete(int id);
    IEnumerable<TestReportDTO> GetAllTestReports();
    IEnumerable<TestReportDTO> GetTestReports(TestReportFilterDTO testReportFilter);
    IEnumerable<string> GetAllWorkstations();
    Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints(ChartInputDataDTO chartInputData);
}
