using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MainProductModel>()
                .HasMany(c => c.ExtraImage)
                .WithOne(e => e.ProductModel);
        }

    }
}