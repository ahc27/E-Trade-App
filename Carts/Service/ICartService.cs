using classLib.CartDtos;
using Carts.Data;

namespace Carts.Service
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart> GetByIdAsync(int id);
        Task<Cart> AddItemToCartAsync(CartDto user);
        Task<Cart> AddAsync(CartDto user);
        Task<bool> UpdateAsync(int id, CartDto user);
        Task<bool> DeleteAsync(int id);
    }
}
