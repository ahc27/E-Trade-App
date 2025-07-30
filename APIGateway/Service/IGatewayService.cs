using APIGateway.Service.Dto;

namespace APIGateway.Service
{
    public interface IGatewayService
    {
        public Task<string> GetAllUsers();

        public Task<string> GetUserById(int id);

        public Task<string> RefreshToken(LoginDto request);

        public Task<string> Login(LoginDto request);


    }
}
