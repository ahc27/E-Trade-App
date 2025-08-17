using AutoMapper;
using Carts.Data;
using classLib.CartDtos;
using classLib.ProductDtos;

namespace Carts.Infrastructures.Mapping
{
    public class CartMapping : Profile
    {
        public CartMapping()
        {
            CreateMap<CartDto, Cart>();
            CreateMap<ProductDto, CartItem>()
                .ForMember(dest => dest.productName, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.unitPrice, opt => opt.MapFrom(src => src.price));
        }
    }
}
