using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TestWatchContext : DbContext
{
    public DbSet<TestReport> TestReports { get; set; }
    public DbSet<Workstation> Workstations { get; set; }
    public TestWatchContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestReport>()
            .ToTable("TestReports")
            .HasKey(t => t.Id);

        modelBuilder.Entity<Workstation>()
            .ToTable("Workstations")
            .HasKey(w => w.Id);

        modelBuilder.Entity<TestReport>()
            .Ignore(t => t.TestSteps);
    }
}
