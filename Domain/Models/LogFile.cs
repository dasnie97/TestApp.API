using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericTestReport.Interfaces;

namespace Domain.Models
{
    [Table("LogFiles")]
    public class LogFile : ILogFile<TestStep>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Workstation { get; set; }
        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string? FixtureSocket { get; set; }
        public string? Failure { get; set; }
        public string? Operator { get; set; }
        public string? TestProgramFilePath { get; set; }
        public bool isFirstPass { get; set;}
        public string ProcessStep { get; set; }
        public List<TestStep>? TestSteps { get; set; }
        public TimeSpan? TestingTime { get; set; }
        public DateTime RecordCreated { get; set; }
        public DateTime TestDateTimeStarted { get; set; }
        
        public LogFile()
        {

        }

        //public LogFile(int id, string workstation, string serialnumber, string fixturesocket, string failure, string testoperator, TimeSpan testingtime, List<TestStep> teststeps)
        //{
        //    (Id, Workstation, SerialNumber, FixtureSocket, Failure, Operator, TestingTime, TestSteps) =
        //        (id, workstation, serialnumber, fixturesocket, failure, testoperator, testingtime, teststeps);
        //}
    }
}
