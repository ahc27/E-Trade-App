using Microsoft.EntityFrameworkCore;

namespace Products.Data
{

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Product> Product { get; set; } 
        }
    
}

