using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Service.Dtos
{
    public class UserAuth
    {
        [Required]
        public string email { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;



    }
}
