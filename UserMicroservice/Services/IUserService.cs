using UserMicroservice.Data;
using UserMicroservice.Services.Dtos;
using classLib;

namespace UserMicroservice.Services
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDto>> GetAllAsync();
        Task<GetUserDto> GetByIdAsync(int id);
        Task<AuthorizationDto> GetByEmailAsync(string email);
        Task<User> AddAsync(CreateUserdto user);
        Task<bool> UpdateRefreshTokenAsync(RefreshTokenDto dto);
        Task<bool> UpdateAsync(int id, UpdateUserdto user);
        Task<bool> DeleteAsync(int id);
    }
}