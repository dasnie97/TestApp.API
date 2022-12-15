namespace Application.DTO.Workstations
{
    public class WorkstationDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Customer { get; set; } = null;
        public string? PositionX { get; set; } = null;
        public string? PositionY { get; set; } = null;
        public string? State { get; set; } = null;
    }
}
