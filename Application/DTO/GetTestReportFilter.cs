namespace Application.DTO;

public class GetTestReportFilter
{
    public string[]? workstation { get; set; } = null;
    public string[]? serialNumber { get; set; } = null;
    public bool? firstPass { get; set; } = null;
    public string? result { get; set; } = null;
    public string[]? dut { get; set; } = null;
    public string[]? failure { get; set; } = null;
    public DateTime? dateFrom { get; set; } = null;
    public DateTime? dateTo { get; set; } = null;
}
