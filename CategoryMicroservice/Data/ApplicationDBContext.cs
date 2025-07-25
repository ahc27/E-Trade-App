using Microsoft.EntityFrameworkCore;

namespace CategoryMicroservice.Data
{

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Category> Category { get; set; } 
        }
    
}

