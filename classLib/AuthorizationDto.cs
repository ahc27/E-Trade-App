namespace classLib
{
    public class AuthorizationDto
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string refreshToken { get; set; }
        public string role { get; set; }

    }
}
