using AutoMapper;
using classLib;
using classLib.LogDtos;

namespace AuthAPI.Infrastructures.Mappers
{
    public class LogMapper : Profile
    {
        public LogMapper() 
        {
            CreateMap<AuthorizationDto,Log >()
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
