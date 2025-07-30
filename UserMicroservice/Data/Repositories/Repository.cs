
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserMicroservice.Data;

namespace UserMicroservice.Data.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }


        public virtual async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public virtual async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public virtual async Task<bool> Update(T entity)
        {
            if (entity == null) return false;

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<T> getByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "email") == email);

        }
    }
}
