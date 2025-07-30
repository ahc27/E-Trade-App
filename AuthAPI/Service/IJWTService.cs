using UserMicroservice.Data;

namespace AuthAPI.Service
{
    public interface IJWTService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken(User user);
        Task<bool> isTokenValid(string token);
    }
}
