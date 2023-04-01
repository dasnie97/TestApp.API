using Application.Interfaces.TestReport;
using Application.Interfaces.Workstations;
using Application.Mappings;
using Application.Services.LogFiles;
using Application.Services.Workstations;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.LogFiles;
using Infrastructure.Repositories.Workstations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPI.WorkerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<ITestReportRepository, TestReportRepository>();
builder.Services.AddScoped<ITestReportService, TestReportService>();
builder.Services.AddScoped<IWorkstationRepository, WorkstationRepository>();
builder.Services.AddScoped<IWorkstationService, WorkstationService>();
builder.Services.AddDbContext<TestWatchContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TestWatch")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title="WebAPI", Version = "v1" });
    c.MapType<TimeSpan>(() => new OpenApiSchema { Type = "string", Format = "time-span" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
