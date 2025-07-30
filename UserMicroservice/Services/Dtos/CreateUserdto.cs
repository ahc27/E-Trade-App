using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Services.Dtos
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
        public string role { get; set; } 
        public string? refreshToken { get; set; }
    }
}
