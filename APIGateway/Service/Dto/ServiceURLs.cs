namespace APIGateway.Service.Dto
{

    public class ServiceUrls
    {
        public UserApiUrls UserApi { get; set; }
        public AuthApiUrls AuthApi { get; set; }
        public CategoryApiUrls CategoryApi { get; set; }

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
        public string DeleteUser { get; set; }
        public string AddUser { get; set; }
        public string UpdateUser { get; set; }
        public string GetUserByEmail { get; set; }
    }

    public class CategoryApiUrls
    {
        public string GetAllCategories { get; set; }
        public string GetCategoryById { get; set; }
        public string DeleteCategory { get; set; }
        public string AddCategory { get; set; }
        public string UpdateCategory { get; set; }
    }
}
