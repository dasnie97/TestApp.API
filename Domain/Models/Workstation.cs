namespace Domain.Models;

public class Workstation
{
    public string Name { get; set; }
    public string? OperatorName { get; set; }
    public string ProcessStep { get; set; }
    public string? State { get; set; }
    public DateTime RecordCreated { get; set; }
    public DateTime RecordUpdated { get; set; }
    public ICollection<TestReport> TestReports { get; set; }
    public ICollection<DowntimeReport> DowntimeReports { get; set; }

    public Workstation(string name = "", string operatorName = "")
    {
        Name = name;
        OperatorName = operatorName;
    }
}
