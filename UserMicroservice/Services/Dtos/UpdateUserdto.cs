using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Services.Dtos
{
    public class UpdateUserdto
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
        public string role { get; set; } = "User"; 
    }
}
