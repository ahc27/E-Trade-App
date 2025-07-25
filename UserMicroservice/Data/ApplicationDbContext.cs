using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace UserMicroservice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } // DbSet for User model
    }
}