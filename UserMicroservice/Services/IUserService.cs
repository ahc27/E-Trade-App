using UserMicroservice.Data;
using UserMicroservice.Services.Dtos;

namespace UserMicroservice.Services
{
    public interface IUserService
    {
        Task<IEnumerable<CreateUserdto>> GetAllAsync();
        Task<CreateUserdto> GetByIdAsync(int id);
        Task<User> AddAsync(CreateUserdto user);
        Task<bool> UpdateAsync(int id, UpdateUserdto user);
        Task<bool> DeleteAsync(int id);
    }
}