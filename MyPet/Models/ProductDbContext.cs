using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;
using MyPet.Areas.Identity.Data;
using System.Xml;

namespace MyPet.Models
{
    public class ProductDbContext : DbContext
    {
        public DbSet<MainProductModel> Products { get; set; }
        public DbSet<ExtraImageModel> ExtraImages { get; set; }
        public DbSet<MainCart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<NewsApiSettingsModel> NewsApiSettings { get; set; }
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

            modelBuilder.Entity<CartProduct>()
              .HasKey(cp => new { cp.CartId, cp.ProductId });

            modelBuilder.Entity<NewsApiSettingsModel>()
            .Property(e => e.Sourses)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<NewsApiSettingsModel>()
            .Property(e => e.Domains)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }

    }
}