using AuthAPI.Service.Dtos;
using UserMicroservice.Data;

namespace AuthAPI.Service
{
        public interface IAuthService
        {
            Task<User> getByEmailAsync(string email);

            Task<string?> login(UserAuth request);
            Task<string> Refresh(UserAuth request);
    } 
}
