using classLib;

namespace AuthAPI.Service
{
        public interface IAuthService
        {
            Task<string?> Login(UserAuth request);
            Task<string> Refresh(UserAuth request);
            Task<bool> LogAuth(string? entityId, bool isLogin,string Action, string message,Exception? exception);
    } 
}
