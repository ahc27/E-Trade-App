using APIGateway.Service.Dto;
using classLib;
using classLib.UserDtos;

namespace APIGateway.Service
{
    public interface IGatewayService
    {
        public Task<List<GetUserDto>?> GetAllUsers(HttpRequest request);

        public Task<string> GetUserById(int id);

        public Task<AuthResponse> RefreshToken(string request);

        public Task<string> Login(LoginDto request);

        public Task<GetCategoryDto> GetCategoryById(int id);

        public Task<string> GetAllCategories();
        public Task<bool> Register(CreateUserdto request);
    }
}
