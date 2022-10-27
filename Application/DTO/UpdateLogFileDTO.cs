namespace Application.DTO
{
    public class UpdateLogFileDTO
    {
        public int Id { get; set; }
        public string Workstation { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
