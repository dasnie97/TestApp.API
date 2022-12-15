using GenericTestReport.Interfaces;

namespace Domain.Models.LogFiles
{
    public class TestStep : ITestStep
    {
        public int Id { get; set; }
        public string TestName { get; set; } = null!;
        public string TestType { get; set; } = null!;
        public DateTime TestDateTimeFinish { get; set; }
        public string TestStatus { get; set; } = null!;
        public string TestValue { get; set; } = null!;
        public string ValueUnit { get; set; } = null!;
        public string TestLowerLimit { get; set; } = null!;
        public string TestUpperLimit { get; set; } = null!;
        public bool IsNumeric { get; set; }
        public string Failure { get; set; } = string.Empty;
        public LogFile Logfile { get; set; } = null!;
        public DateTime RecordCreated { get; set; }


        public TestStep() { }

        public TestStep(int id, string name, string type, string value, string unit, string lowerlimit, string upperlimit, LogFile logfile)
        {
            (Id, TestName, TestType, TestValue, ValueUnit, TestLowerLimit, TestUpperLimit, Logfile) = (id, name, type, value, unit, lowerlimit, upperlimit, logfile);
        }
    }
}
