using classLib;

namespace AuthAPI.Service
{
        public interface IAuthService
        {
            Task<AuthResponse> Login(UserAuth request);
            Task<AuthResponse> Refresh(string request);
            Task<bool> LogAuth(string? entityId, bool isLogin,string Action, string message,Exception? exception);
    } 
}
