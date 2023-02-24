using Microsoft.EntityFrameworkCore;

namespace MyPet.Models
{
    public class ProductDbContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
                : base(options)
        {
            Database.EnsureCreated();   
        }
    }
}