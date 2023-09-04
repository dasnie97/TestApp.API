namespace Domain.Models;

public class DowntimeReport
{
    public int Id { get; set; }
    public string ProblemDescription { get; set; }
    public string ActionTaken { get; set; }
    public string Technician { get; set; }
    public Workstation Workstation { get; set; }
    public string WorkstationName { get; set; }
    public string Operator { get; set; }
    public DateTime TimeStarted { get; set; }
    public DateTime TimeFinished { get; set; }
    public TimeSpan TotalDowntime { get; set; }
    public DateTime RecordCreated { get; set; }
}
