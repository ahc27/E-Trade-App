using AutoMapper;
using UserMicroservice.Data;
using UserMicroservice.Services.Dtos;

namespace UserMicroservice.Infrastructures.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
        CreateMap<CreateUserdto, User>().ReverseMap();
        CreateMap<UpdateUserdto, User>().ReverseMap();
        }
    }
}
