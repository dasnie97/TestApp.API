namespace Domain.Models
{
    public class GetLogFilesQuery
    {
        public string[]? Workstation { get; set; }
        public string[]? SerialNumber { get; set; }
        public bool? firstPass { get; set; }
        public TestStatus Result { get; set; }
        public string[]? Dut { get; set; }
        public string[]? Failure { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
