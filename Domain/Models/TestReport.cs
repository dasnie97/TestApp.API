using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("TestReports")]
public class TestReport
{
    [Key]
    public int Id { get; private set; }
    public List<TestStep> TestSteps { get; set; }
    public bool IsFirstPass { get; set; }
    public string? ProcessStep { get; set; }
    public string Workstation { get; set; }
    public DateTime RecordCreated { get; set; }
    public string SerialNumber { get; set; }
    public DateTime TestDateTimeStarted { get; set; }
    public string Status { get; set; }
    public string Failure { get; set; }
    public string? FixtureSocket { get; set; }
    public TimeSpan? TestingTime { get; set; }
}
