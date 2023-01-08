using Application.DTO.TestReport;
using Application.DTO.Workstations;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TestReport, TestReportDTO>();
            cfg.CreateMap<CreateTestReportDTO, TestReport>();
            cfg.CreateMap<UpdateTestReportDTO, TestReport>();
            cfg.CreateMap<GetTestReportFilter, GetLogFilesQuery>();
            cfg.CreateMap<Workstation, WorkstationDTO>();
            cfg.CreateMap<AddWorkstationDTO, Workstation>();
            cfg.CreateMap<WorkstationDTO, Workstation>();
        }).CreateMapper();
    }
}
