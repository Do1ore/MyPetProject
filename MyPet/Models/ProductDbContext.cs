using Microsoft.EntityFrameworkCore;

namespace MyPet.Models
{
    public class ProductDbContext : DbContext
    {
        public DbSet<MainProductModel> Products { get; set; } 
        public DbSet<ExtraImageModel> ExtraImages { get; set; }
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }
    }
}