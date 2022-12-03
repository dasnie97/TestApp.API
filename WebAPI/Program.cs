using Application.Interfaces.LogFiles;
using Application.Interfaces.Workstations;
using Application.Mappings;
using Application.Services.LogFiles;
using Application.Services.Workstations;
using Domain.Interfaces.LogFiles;
using Domain.Interfaces.Workstations;
using Infrastructure.Data;
using Infrastructure.Repositories.LogFiles;
using Infrastructure.Repositories.Workstations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ILogFileRepository, LogFileRepository>();
builder.Services.AddScoped<ILogFileService, LogFileService>();
builder.Services.AddScoped<IWorkstationRepository, WorkstationRepository>();
builder.Services.AddScoped<IWorkstationService, WorkstationService>();
builder.Services.AddDbContext<TestWatchContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TestWatch")));

builder.Services.AddSingleton(AutoMapperConfig.Initialize());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title="WebAPI", Version = "v1" });
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
