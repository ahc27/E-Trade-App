using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("userId")]
        public int userId { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
        public bool isActive { get; set; } = true;

        public List<CartItem> Items { get; set; } = new();
    }
}
