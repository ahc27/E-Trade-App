using AutoMapper;
using CategoryMicroservice.Data;
using CategoryMicroservice.Service.Dtos;
using classLib;

namespace CategoryMicroservice.Infrastructures.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
            CreateMap<GetCategoryDto, Category>().ReverseMap();
        }
    }
}
