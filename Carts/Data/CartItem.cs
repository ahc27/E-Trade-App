using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carts.Data
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("cartId")]
        public int cartId { get; set; }

        [ForeignKey(nameof(productId))]
        public int productId { get; set; }
        public string productName { get; set; }
        public int unitPrice { get; set; }
        public int quantity { get; set; }
        public decimal TotalPrice => unitPrice * quantity;
    }
}
