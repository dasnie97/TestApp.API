using Application.DTO;
using TestEngineering.DTO;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TestReport, TestReportDTO>().ForMember(dest=>dest.Workstation, opt=>opt.MapFrom(src=>src.Workstation.Name));
        CreateMap<CreateTestReportDTO, TestReport>().ForMember(dest => dest.Workstation, opt=>opt.MapFrom(src=>new Workstation(src.Workstation, "")));
        CreateMap<UpdateTestReportDTO, TestReport>().ForMember(dest => dest.Workstation, opt => opt.MapFrom(src => new Workstation(src.Workstation, "")));
        CreateMap<TestReportFilterDTO, TestReportFilter>();

        CreateMap<DowntimeReport, DowntimeReportDTO>().ForMember(dest => dest.Workstation, opt => opt.MapFrom(src => src.Workstation.Name));
        CreateMap<CreateDowntimeReportDTO, DowntimeReport>().ForMember(dest => dest.Workstation, opt => opt.MapFrom(src => new Workstation(src.Workstation, "")));
        CreateMap<UpdateDowntimeReportDTO, DowntimeReport>().ForMember(dest => dest.Workstation, opt => opt.MapFrom(src => new Workstation(src.Workstation, "")));
        CreateMap<DowntimeReportFilterDTO, DowntimeReportFilter>();

        CreateMap<WorkstationFilterDTO, WorkstationFilter>();
        CreateMap<Workstation, WorkstationDTO>();
        CreateMap<CreateWorkstationDTO, Workstation>();
        CreateMap<WorkstationDTO, Workstation>();
        CreateMap<ChartInputDataDTO, ChartInputData>();
    }
}