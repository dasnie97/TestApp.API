using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.LogFiles
{
    public class GetLogFilesFilter
    {
        public string[]? workstation { get; set; } = null;
        public string[]? serialNumber { get; set; } = null;
        public string? result { get; set; } = null;
        public string[]? dut { get; set; } = null;
        public string[]? failure { get; set; } = null;
        public DateTime? dateFrom { get; set; } = null;
        public DateTime? dateTo { get; set; } = null;
    }
}
