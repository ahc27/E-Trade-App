using AutoMapper;
using Business.Dtos;
using classLib.LogDtos;

namespace LoggingAPI.Infrastructure.Mappers
{
    public class LogMapper : Profile
    {
        public LogMapper() 
        {
            CreateMap<Log, LogInfo>();
        }
    }
}
