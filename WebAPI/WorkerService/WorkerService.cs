using Application.DTO;
using Application.Interfaces;

namespace WebAPI.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _provider;

    public Worker(ILogger<Worker> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _provider.CreateScope())
            {
                var workstationService = scope.ServiceProvider.GetRequiredService<IWorkstationService>();
                var testReportService = scope.ServiceProvider.GetRequiredService<ITestReportService>();
                var filter = new TestReportFilterDTO();
                filter.dateFrom = DateTime.Now.AddMinutes(-20);
                filter.firstPass = true;
                filter.result = "Passed";
                
                var workstations = workstationService.GetAll();
                foreach (var workstation in workstations)
                {
                    filter.workstation = new string[] {workstation.Name};
                    var logFiles = testReportService.GetTestReports(filter);
                    if (logFiles.Count() == 0)
                    {
                        workstation.State = "Idle";
                        workstationService.Update(workstation);
                    }
                }
            }
            await Task.Delay(1200000, stoppingToken);
        }
    }
}
