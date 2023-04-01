using Application.DTO.TestReport;
using Application.DTO.Workstations;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TestReport, TestReportDTO>().ReverseMap();
        CreateMap<CreateTestReportDTO, TestReport>().ForMember(dest => dest.Workstation, opt => opt.MapFrom(src => new Workstation(src.WorkstationName, string.Empty)));
        CreateMap<UpdateTestReportDTO, TestReport>();
        CreateMap<GetTestReportFilter, GetLogFilesQuery>();
        CreateMap<Workstation, WorkstationDTO>();
        CreateMap<AddWorkstationDTO, Workstation>();
        CreateMap<WorkstationDTO, Workstation>();
    }
}