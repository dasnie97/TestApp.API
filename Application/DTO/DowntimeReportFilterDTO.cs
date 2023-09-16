namespace Application.DTO;

public class DowntimeReportFilterDTO
{
    public string[]? ProblemDescription { get; set; } = null;
    public string[]? ActionTaken { get; set; } = null;
    public string[]? Technician { get; set; } = null;
    public string[]? Workstation { get; set; } = null;
    public string[]? Operator { get; set; } = null;
    public DateTime? DateFrom { get; set; } = null;
    public DateTime? DateTo { get; set; } = null;
}
