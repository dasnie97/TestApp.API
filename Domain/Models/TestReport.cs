using ProductTest.Common;

namespace Domain.Models;

public class TestReport : ProductTest.Models.TestReport
{
    public int Id { get; private set; }
    public bool IsFirstPass { get; set; }
    public bool IsFalseCall { get; set; }
    //TODO: Make this of WorkstationBase type
    public new string Workstation { get; set; }
    public string? ProcessStep { get; set; }
    public DateTime RecordCreated { get; set; }
}
