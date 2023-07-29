namespace Infrastructure.Helpers;

public class TimeInterval
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public TimeInterval(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }
}
