using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class TestWatchContext : DbContext
{
    public DbSet<TestReport> TestReports { get; set; }
    public DbSet<Workstation> Workstations { get; set; }

    private IConfiguration _configuration;

    public TestWatchContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbType = _configuration["DBType"];
        if (dbType == "SQLServer")
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SQLServer"));
        }
        else if (dbType == "MySQL")
        {
            optionsBuilder.UseMySQL(_configuration.GetConnectionString("MySQL"));
        }
        else
        {
            throw new Exception("DB type not chosen! Choose either 'SQLServer' or 'MySQL' in appsettings.json");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestReport>()
            .ToTable("TestReports")
            .HasKey(t => t.Id);

        modelBuilder.Entity<Workstation>()
            .ToTable("Workstations")
            .HasKey(w => w.Name);

        modelBuilder.Entity<Workstation>()
            .HasMany(w => w.TestReports)
            .WithOne(t => t.Workstation)
            .HasForeignKey(w => w.WorkstationName)
            .HasPrincipalKey(t => t.Name);

        modelBuilder.Entity<TestReport>()
            .Property(t=>t.Status)
            .HasConversion<string>();

    }
}
