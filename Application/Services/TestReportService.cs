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

    public TestReportDTO Add(CreateTestReportDTO logFile)
    {
        if (string.IsNullOrEmpty(logFile.SerialNumber)) throw new Exception("Test report has to have serial number!");
        var mappedLogFile = _mapper.Map<TestReport>(logFile);
        _testReportRepository.Add(mappedLogFile);
        return _mapper.Map<TestReportDTO>(mappedLogFile);
    }

    public TestReportDTO GetTestReportByID(int id)
    {
        var testReport = _testReportRepository.Get(id);
        return _mapper.Map<TestReportDTO>(testReport);
    }

    public void Update(UpdateTestReportDTO updateTestReportDTO)
    {
        var originalLogFile = _testReportRepository.Get(updateTestReportDTO.Id);
        var logFile = _mapper.Map(updateTestReportDTO, originalLogFile);
        _testReportRepository.Update(logFile);
    }

    public void Delete(int id)
    {
        var logFile = _testReportRepository.Get(id);
        _testReportRepository.Delete(logFile);
    }

    public IEnumerable<TestReportDTO> GetAllTestReports()
    {
        var getTestReportsFilter = new TestReportFilterDTO();
        var filter = _mapper.Map<TestReportFilter>(getTestReportsFilter);
        var filteredLogFiles = _testReportRepository.Get(filter);
        return _mapper.Map<IEnumerable<TestReportDTO>>(filteredLogFiles);
    }

    public IEnumerable<TestReportDTO> GetTestReports(TestReportFilterDTO getTestReportsFilter)
    {
        var filter = _mapper.Map<TestReportFilter>(getTestReportsFilter);
        var filteredLogFiles = _testReportRepository.Get(filter);
        return _mapper.Map<IEnumerable<TestReportDTO>>(filteredLogFiles);
    }

    public IEnumerable<string> GetAllWorkstations()
    {
        return _mapper.Map<IEnumerable<string>>(_testReportRepository.GetAllWorkstations());
    }

    public Dictionary<string, IEnumerable<YieldPoint>> GetYieldPoints()
    {
        return _testReportRepository.GetYieldPoints();
    }
}
