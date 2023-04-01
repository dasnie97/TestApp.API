using ProductTest.Common;

namespace Domain.Models;

public class TestReport
{
    public int Id { get; private set; }
    public bool IsFirstPass { get; set; }
    public bool IsFalseCall { get; set; }
    public string? ProcessStep { get; set; }
    public string Workstation { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; }
    public string Failure { get; set; }
    public string? FixtureSocket { get; set; }
    public DateTime TestDateTimeStarted { get; set; }
    public TimeSpan? TestingTime { get; set; }
    public DateTime RecordCreated { get; set; }
}
