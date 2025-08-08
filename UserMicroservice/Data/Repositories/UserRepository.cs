using Microsoft.EntityFrameworkCore;

namespace UserMicroservice.Data.Repositories
{
    public class UserRepository : Repository<User>
    {
        private readonly ApplicationDbContext _context;

        
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
    
        }


        public async Task<IEnumerable<User>> GetAll()
        {
            return await base.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await base.GetById(id);
        }

        public  async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<User> Add(User UserEntity)
        {
            return await base.Add(UserEntity);
        }


        public async Task<bool> Delete(int id)
        {
            return await base.Delete(id);
        }

        public async Task<User> Update(User UserEntity)
        {

            if (UserEntity == null)
            {
                return null;
            }

            var existingUser = await _context.Users.FindAsync(UserEntity.Id);
            if (existingUser == null) return null;

            return await base.Update(UserEntity);

        }
        public async Task<bool> UpdateRefreshTokenAsync(User user)
        {
            var isSuccess = await base.UpdateRefreshTokenAsync(user);
            return isSuccess;
        }
        public async Task<User> getByEmail(string email)
        {
            return await base.getByEmail(email);
        }
    }
}
