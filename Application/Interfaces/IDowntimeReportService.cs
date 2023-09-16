using Application.DTO;
using TestEngineering.DTO;

namespace Application.Interfaces;

public interface IDowntimeReportService
{
    DowntimeReportDTO Add(CreateDowntimeReportDTO downtimeReportDTO);
    DowntimeReportDTO GetDowntimeReportByID(int id);
    void Update(UpdateDowntimeReportDTO downtimeReportDTO);
    void Delete(int id);
    IEnumerable<DowntimeReportDTO> GetDowntimeReports(DowntimeReportFilterDTO downtimeReportFilter);
}
