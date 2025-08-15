using Microsoft.EntityFrameworkCore;

namespace Products.Data.Repositories
{
    public class ProductRepository 
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _dbSet;


        public ProductRepository(ApplicationDbContext context) 
        {
            _context = context;
            _dbSet = context.Product;

        }


        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int?> GetByName(String Name)
        {
           var entity = await _dbSet.FirstOrDefaultAsync(x => x.name == Name);
            if (entity == null) return null;
            return entity.Id;
        }

        public async Task<Product> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Product> Add(Product ProductEntity)
        {

            await _dbSet.AddAsync(ProductEntity);
            await _context.SaveChangesAsync();
            return ProductEntity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> Update(Product ProductEntity)
        {

            if (ProductEntity == null)
            {
                return null;
            }

            var existingProduct = await _dbSet.FindAsync(ProductEntity.Id);
            if (existingProduct == null) return null;

            _dbSet.Update(existingProduct);
            await _context.SaveChangesAsync();
            return existingProduct;

        }
    }
}
