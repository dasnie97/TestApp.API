using Application.DTO;
using TestEngineering.DTO;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Application.Interfaces;

namespace Application.Services;

public class TestReportService : ITestReportService
{
    private readonly ITestReportRepository _testReportRepository;
    private readonly IMapper _mapper;
    public TestReportService(ITestReportRepository testReportRepository, IMapper mapper)
    {
        this._testReportRepository = testReportRepository;
        _mapper = mapper;
    }

    public TestReportDTO Add(CreateTestReportDTO testreport)
    {
        if (string.IsNullOrEmpty(testreport.SerialNumber)) throw new Exception("Test report has to have serial number!");
        var mappedtestreport = _mapper.Map<TestReport>(testreport);
        _testReportRepository.Add(mappedtestreport);
        return _mapper.Map<TestReportDTO>(mappedtestreport);
    }

    public TestReportDTO GetTestReportByID(int id)
    {
        var testReport = _testReportRepository.Get(id);
        return _mapper.Map<TestReportDTO>(testReport);
    }

    public void Update(UpdateTestReportDTO updateTestReportDTO)
    {
        var originaltestreport = _testReportRepository.Get(updateTestReportDTO.Id);
        var testreport = _mapper.Map(updateTestReportDTO, originaltestreport);
        _testReportRepository.Update(testreport);
    }

    public void Delete(int id)
    {
        var testreport = _testReportRepository.Get(id);
        _testReportRepository.Delete(testreport);
    }

    public IEnumerable<TestReportDTO> GetAllTestReports()
    {
        var getTestReportsFilter = new TestReportFilterDTO();
        var filter = _mapper.Map<TestReportFilter>(getTestReportsFilter);
        var filteredtestreports = _testReportRepository.Get(filter);
        return _mapper.Map<IEnumerable<TestReportDTO>>(filteredtestreports);
    }

    public IEnumerable<TestReportDTO> GetTestReports(TestReportFilterDTO getTestReportsFilter)
    {
        var filter = _mapper.Map<TestReportFilter>(getTestReportsFilter);
        var filteredtestreports = _testReportRepository.Get(filter);
        return _mapper.Map<IEnumerable<TestReportDTO>>(filteredtestreports);
    }

    public IEnumerable<string> GetAllWorkstations()
    {
        return _mapper.Map<IEnumerable<string>>(_testReportRepository.GetAllWorkstations());
    }

    public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints(ChartInputDataDTO chartInputDataDTO)
    {
        var chartInputData = _mapper.Map<ChartInputData>(chartInputDataDTO);
        var yieldPoints = _testReportRepository.GetYieldPoints(chartInputData);
        return yieldPoints;
    }
}
