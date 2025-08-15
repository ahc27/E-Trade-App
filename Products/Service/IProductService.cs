using classLib.ProductDtos;
using Products.Data;

namespace Products.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<Product> AddAsync(ProductDto user);
        Task<bool> UpdateAsync(int id, ProductDto user);
        Task<bool> DeleteAsync(int id);
    }
}
