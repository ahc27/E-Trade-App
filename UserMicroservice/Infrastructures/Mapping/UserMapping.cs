using AutoMapper;
using UserMicroservice.Data;
using UserMicroservice.Services.Dtos;
using classLib;
using classLib.UserDtos;

namespace UserMicroservice.Infrastructures.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
        CreateMap<CreateUserdto, User>().ReverseMap();
        CreateMap<UpdateUserdto, User>().ReverseMap();
        CreateMap<AuthorizationDto,User >().ReverseMap();
        CreateMap<User, GetUserDto>();

        }
    }
}
