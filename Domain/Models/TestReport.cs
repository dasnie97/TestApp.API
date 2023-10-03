namespace Domain.Models;

public class TestReport
{
    public int Id { get; set; }
    public Workstation Workstation { get; set; }
    public string WorkstationName { get; set; }
    public string SerialNumber { get; set; }
    public TestStatus Status { get; set; }
    public DateTime TestDateTimeStarted { get; set; }
    public TimeSpan? TestingTime { get; set; }
    public string? FixtureSocket { get; set; }
    public string Failure { get; set; }
    public bool IsFirstPass { get; set; }
    public bool? IsFalseCall { get; set; }
    public DateTime RecordCreated { get; set; }
}
