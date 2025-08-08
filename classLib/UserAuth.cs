using System.ComponentModel.DataAnnotations;

namespace classLib
{
    public class UserAuth
    {
        [Required]
        public string email { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;
    }
}
