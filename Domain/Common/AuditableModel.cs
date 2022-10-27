namespace Domain.Common
{
    public abstract class AuditableModel
    {
        public string Status { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime ExecutionStarted { get; set; }
    }
}
