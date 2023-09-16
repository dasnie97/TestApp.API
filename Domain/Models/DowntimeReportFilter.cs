namespace Domain.Models;

public class DowntimeReportFilter
{
    public string[] ProblemDescription { get; set; }
    public string[] ActionTaken { get; set; }
    public string[] Technician { get; set; }
    public string[] Workstation { get; set; }
    public string[] Operator { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
