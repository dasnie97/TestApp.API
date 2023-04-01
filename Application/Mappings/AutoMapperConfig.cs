using Application.DTO.TestReport;
using Application.DTO.Workstations;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TestReport, TestReportDTO>();
        CreateMap<CreateTestReportDTO, TestReport>();
        CreateMap<UpdateTestReportDTO, TestReport>();
        CreateMap<GetTestReportFilter, GetLogFilesQuery>();
        CreateMap<Workstation, WorkstationDTO>();
        CreateMap<AddWorkstationDTO, Workstation>();
        CreateMap<WorkstationDTO, Workstation>();
    }
}