using Microsoft.EntityFrameworkCore;

namespace Carts.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; } 
        public DbSet<CartItem> CartItems { get; set; } 
    }
}