using Microsoft.EntityFrameworkCore;

namespace Carts.Data.Repositories
{
    public class CartItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<CartItem> _dbSet;


        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.CartItems;

        }


        public async Task<IEnumerable<CartItem>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<CartItem> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<CartItem> Add(CartItem CartItemEntity)
        {

            await _dbSet.AddAsync(CartItemEntity);
            await _context.SaveChangesAsync();
            return CartItemEntity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem> Update(CartItem CartItemEntity)
        {

            if (CartItemEntity == null)
            {
                return null;
            }

            var existingCartItem = await _dbSet.FindAsync(CartItemEntity.Id);
            if (existingCartItem == null) return null;

            _dbSet.Update(existingCartItem);
            await _context.SaveChangesAsync();
            return existingCartItem;

        }
    }
}
