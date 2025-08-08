using APIGateway.Service.Dto;
using classLib;

namespace APIGateway.Service
{
    public interface IGatewayService
    {
        public Task<List<GetUserDto>> GetAllUsers(HttpRequest request);

        public Task<string> GetUserById(int id);

        public Task<string> RefreshToken(LoginDto request);

        public Task<string> Login(LoginDto request);

        public Task<string> GetCategoryById(int id);

        public Task<string> GetAllCategories();

    }
}
