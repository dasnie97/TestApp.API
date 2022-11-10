namespace Domain.Models
{
    public class YieldPoint
    {
        public DateTime DateAndTime { get; set; }
        public float Yield { get; set; }
        public int Total { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
    }
}
