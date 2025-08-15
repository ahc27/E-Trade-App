using AutoMapper;
using Products.Data;
using classLib.ProductDtos;

namespace Products.Infrastructures.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
