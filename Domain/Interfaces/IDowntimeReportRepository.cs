using Domain.Models;

namespace Domain.Interfaces;

public interface IDowntimeReportRepository
{
    DowntimeReport Add(DowntimeReport downtimeReport);
    DowntimeReport Get(int id);
    void Update(DowntimeReport downtimeReport);
    void Delete(DowntimeReport downtimeReport);
    IEnumerable<DowntimeReport> Get(DowntimeReportFilter downtimeReportFilter);
}
