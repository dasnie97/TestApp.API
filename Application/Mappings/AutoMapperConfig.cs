using Application.DTO.TestReport;
using Application.DTO.Workstations;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TestReport, TestReportDTO>();//.ForMember(dest => dest.Workstation, opt => opt.MapFrom(src => src.Workstation));
        CreateMap<CreateTestReportDTO, TestReport>();//.ForMember(dest => dest.Workstation, opt => opt.MapFrom(src => new Workstation(src.Workstation, string.Empty)));
        CreateMap<UpdateTestReportDTO, TestReport>();
        CreateMap<GetTestReportFilter, GetLogFilesQuery>();
        CreateMap<Workstation, WorkstationDTO>();
        CreateMap<AddWorkstationDTO, Workstation>();
        CreateMap<WorkstationDTO, Workstation>();
    }
}