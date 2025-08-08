using classLib;

namespace AuthAPI.Service
{
    public interface IJWTService
    {
        string GenerateToken(AuthorizationDto user);
        string GenerateRefreshToken(AuthorizationDto user);
        Task<bool> isTokenValid(string token);
    }
}
