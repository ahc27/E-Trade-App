using AutoMapper;
using CategoryMicroservice.Data;
using CategoryMicroservice.Service.Dtos;

namespace CategoryMicroservice.Infrastructures.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
        }
    }
}
