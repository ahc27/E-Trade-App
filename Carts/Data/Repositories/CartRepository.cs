using Microsoft.EntityFrameworkCore;

namespace Carts.Data.Repositories
{
    public class CartRepository 
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Cart> _dbSet;


        public CartRepository(ApplicationDbContext context) 
        {
            _context = context;
            _dbSet = context.Carts;

        }


        public async Task<IEnumerable<Cart>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Cart> GetByUserID(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.userId == id && x.isActive == true);
            if (entity == null) return null;
            return entity;
        }

        public async Task<Cart> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Cart> Add(Cart CartEntity)
        {

            await _dbSet.AddAsync(CartEntity);
            await _context.SaveChangesAsync();
            return CartEntity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Cart> Update(Cart CartEntity)
        {

            if (CartEntity == null)
            {
                return null;
            }

            var existingCart = await _dbSet.FindAsync(CartEntity.Id);
            if (existingCart == null) return null;

            _dbSet.Update(existingCart);
            await _context.SaveChangesAsync();
            return existingCart;

        }
    }
}
