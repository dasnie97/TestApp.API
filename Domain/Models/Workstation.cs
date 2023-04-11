namespace Domain.Models;

public class Workstation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? OperatorName { get; set; }
    public string? Customer { get; set; }
    public string? PositionX { get; set; }
    public string? PositionY { get; set; }
    public string? State { get; set; }
    public DateTime RecordCreated { get; set; }
    public DateTime RecordUpdated { get; set; }
    public ICollection<TestReport> TestReports { get; set; }

    public Workstation(string name = "", string operatorName = "")
    {
        Name = name;
        OperatorName = operatorName;
    }
}
