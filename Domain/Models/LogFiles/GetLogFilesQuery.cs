namespace Domain.Models.LogFiles
{
    public class GetLogFilesQuery
    {
        public string[] workstation { get; set; }
        public string[] serialNumber { get; set; }
        public string result { get; set; }
        public string[] dut { get; set; }
        public string[] failure { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
    }
}
