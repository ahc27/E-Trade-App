namespace APIGateway.Service.Dto
{

    public class ServiceUrls
    {
        public UserApiUrls UserApi { get; set; }
        public AuthApiUrls AuthApi { get; set; }
    }

    public class AuthApiUrls
    {
        public string Login { get; set; }
        public string Refresh { get; set; }
    }
    public class UserApiUrls
    {
        public string GetAllUsers { get; set; }
        public string GetUserById { get; set; }
    }
}
