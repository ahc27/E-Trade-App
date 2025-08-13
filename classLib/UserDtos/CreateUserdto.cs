using System.ComponentModel.DataAnnotations;

namespace classLib.UserDtos
{
    public class CreateUserdto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public bool isSeller { get; set; }
        public string? refreshToken { get; set; }
    }
}
