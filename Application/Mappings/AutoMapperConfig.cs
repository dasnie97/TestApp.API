using Application.DTO.LogFiles;
using Application.DTO.Workstations;
using AutoMapper;
using Domain.Models.LogFiles;
using Domain.Models.Workstations;

namespace Application.Mappings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LogFile, LogFileDTO>();
            cfg.CreateMap<CreateLogFileDTO, LogFile>();
            cfg.CreateMap<UpdateLogFileDTO, LogFile>();
            cfg.CreateMap<GetLogFilesFilter, GetLogFilesQuery>();
            cfg.CreateMap<Workstation, WorkstationDTO>();
            cfg.CreateMap<AddWorkstationDTO, Workstation>();
            cfg.CreateMap<WorkstationDTO, Workstation>();
        }).CreateMapper();
    }
}
