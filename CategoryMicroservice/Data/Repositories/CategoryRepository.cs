using Microsoft.EntityFrameworkCore;

namespace CategoryMicroservice.Data.Repositories
{
    public class CategoryRepository 
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Category> _dbSet;


        public CategoryRepository(ApplicationDbContext context) 
        {
            _context = context;
            _dbSet = context.Category;

        }


        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int?> GetByName(String Name)
        {
           var entity = await _dbSet.FirstOrDefaultAsync(x => x.Name == Name);
            if (entity == null) return null;
            return entity.Id;
        }

        public async Task<Category> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Category> Add(Category CategoryEntity)
        {

            await _dbSet.AddAsync(CategoryEntity);
            await _context.SaveChangesAsync();
            return CategoryEntity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Category> Update(Category CategoryEntity)
        {

            if (CategoryEntity == null)
            {
                return null;
            }

            var existingCategory = await _dbSet.FindAsync(CategoryEntity.Id);
            if (existingCategory == null) return null;

            _dbSet.Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;

        }
    }
}
