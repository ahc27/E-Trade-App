
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

        public async Task<User> Add(User UserEntity)
        {
            return await base.Add(UserEntity);
        }

        public async Task<bool> Delete(int id)
        {
            return await base.Delete(id);
        }

        public async Task<bool> Update(User UserEntity)
        {

            if (UserEntity == null)
            {
                return false;
            }

            var existingUser = await _context.Users.FindAsync(UserEntity.Id);
            if (existingUser == null) return false;

            return await base.Update(UserEntity);

        }
    }
}
