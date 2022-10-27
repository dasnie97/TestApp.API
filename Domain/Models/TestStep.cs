using Domain.Common;

namespace Domain.Models
{
    public class TestStep : AuditableModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public string LowerLimit { get; set; } = null!;
        public string UpperLimit { get; set; } = null!;
        public LogFile Logfile { get; set; } = null!;

        public TestStep(){}

        public TestStep(int id, string name, string type, string value, string unit, string lowerlimit, string upperlimit, LogFile logfile)
        {
            (Id, Name, Type, Value, Unit, LowerLimit, UpperLimit, Logfile) = (id, name, type, value, unit, lowerlimit, upperlimit, logfile);
        }
    }
}
