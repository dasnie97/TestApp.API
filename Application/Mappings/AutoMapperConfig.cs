using Application.DTO;
using AutoMapper;
using Domain.Models;

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
        }).CreateMapper();
    }
}
