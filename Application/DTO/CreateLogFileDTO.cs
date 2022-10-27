namespace Application.DTO
{
    public class CreateLogFileDTO
    {
        public string Workstation { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
