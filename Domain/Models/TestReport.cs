using ProductTest.Common;
using ProductTest.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("TestReports")]
public class TestReport : TestReportBase
{
    [Key]
    public int Id { get; private set; }
    public new Workstation Workstation { get; set; }
    public new List<TestStep> TestSteps { get; set; }
    public bool IsFirstPass { get; set; }
    public string? ProcessStep { get; set; }
    public DateTime RecordCreated { get; set; }

    public TestReport() : base(string.Empty,
                                TestStatus.Error,
                                string.Empty,
                                DateTime.MinValue,
                                new List<TestStepBase>())
    { }
}
