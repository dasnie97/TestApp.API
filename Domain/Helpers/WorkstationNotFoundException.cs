namespace Domain.Helpers;

public class WorkstationNotFoundException : Exception
{
    public WorkstationNotFoundException()
    {
        
    }

    public WorkstationNotFoundException(string message) : base(message)
    {
        
    }
}
