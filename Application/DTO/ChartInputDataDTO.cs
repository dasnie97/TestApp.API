namespace Application.DTO;

public class ChartInputDataDTO
{
    public string[]? workstation { get; set; } = null;
    public DateTime? dateFrom { get; set; } = null;
    public DateTime? dateTo { get; set; } = null;
}
