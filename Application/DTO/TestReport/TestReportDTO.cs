namespace Application.DTO.TestReport;

public class TestReportDTO
{
    public int Id { get; set; }
    public string? Workstation { get; set; } = null!;
    public string? SerialNumber { get; set; } = null!;
    public string? Status { get; set; } = null!;
    public string? FixtureSocket { get; set; }
    public string? Failure { get; set; }
    public string? ProcessStep { get; set; }
    public DateTime TestDateTimeStarted { get; set; }
    public TimeSpan? TestingTime { get; set; }
}
