using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.LogFiles;
using Domain.Models.Workstations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class TestWatchContext : DbContext
    {
        public DbSet<LogFile> LogFiles { get; set; }
        public DbSet<Workstation> Workstations { get; set; }
        public TestWatchContext(DbContextOptions options) : base(options)
        {

        }
    }
}
