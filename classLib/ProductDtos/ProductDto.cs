using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace classLib.ProductDtos
{
    public class ProductDto
    {

        [ForeignKey("sellerId")]
        public int sellerId { get; set; }

        [ForeignKey("categoryId")]
        public int categoryId{ get; set; }
        [Required]
        public string name { get; set; } 
        public string? description { get; set; } 
        public string? brand { get; set; } 
        public DateTime? createdAt { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public int stockQuantity { get; set; }
    }
}
