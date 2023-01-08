using ProductTest.Common;
using ProductTest.Models;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class TestStep : TestStepBase
{
    [Key]
    public int Id { get; private set; }
    public TestReport TestReport { get; set; }
    public DateTime RecordCreated { get; set; }

    public TestStep() : base(string.Empty, DateTime.MinValue, TestStatus.Error)
    { }
}
