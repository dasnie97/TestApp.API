using Application.DTO;
using TestEngineering.DTO;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Application.Interfaces;

namespace Application.Services;

public class DowntimeReportService : IDowntimeReportService
{
    private readonly IDowntimeReportRepository _downtimeReportRepository;
    private readonly IMapper _mapper;
    public DowntimeReportService(IDowntimeReportRepository downtimeReportRepository, IMapper mapper)
    {
        _downtimeReportRepository = downtimeReportRepository;
        _mapper = mapper;
    }

    public DowntimeReportDTO Add(CreateDowntimeReportDTO downtimeReport)
    {
        if (string.IsNullOrEmpty(downtimeReport.ProblemDescription)) throw new Exception("Downtime report has to have description defined!");
        var mappedDowntimeReport = _mapper.Map<DowntimeReport>(downtimeReport);
        _downtimeReportRepository.Add(mappedDowntimeReport);
        return _mapper.Map<DowntimeReportDTO>(mappedDowntimeReport);
    }

    public DowntimeReportDTO GetDowntimeReportByID(int id)
    {
        var downtimeReport = _downtimeReportRepository.Get(id);
        return _mapper.Map<DowntimeReportDTO>(downtimeReport);
    }

    public void Update(UpdateDowntimeReportDTO updateDowntimeReportDTO)
    {
        var originalDowntimeReport = _downtimeReportRepository.Get(updateDowntimeReportDTO.Id);
        var downtimeReport = _mapper.Map(updateDowntimeReportDTO, originalDowntimeReport);
        _downtimeReportRepository.Update(downtimeReport);
    }

    public void Delete(int id)
    {
        var downTimeReport = _downtimeReportRepository.Get(id);
        _downtimeReportRepository.Delete(downTimeReport);
    }

    public IEnumerable<DowntimeReportDTO> GetDowntimeReports(DowntimeReportFilterDTO getDowntimeReportsFilter)
    {
        var filter = _mapper.Map<DowntimeReportFilter>(getDowntimeReportsFilter);
        var filteredDowntimeReports = _downtimeReportRepository.Get(filter);
        return _mapper.Map<IEnumerable<DowntimeReportDTO>>(filteredDowntimeReports);
    }
}
