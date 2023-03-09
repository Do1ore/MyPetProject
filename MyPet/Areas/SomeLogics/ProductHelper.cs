using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyPet.Models;

namespace MyPet.Areas.SomeLogics
{
    public class ProductHelper
    {
        private static ProductDbContext? db;
        public static async Task<List<string?>> GetAllCategoriesAsync()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
                .Options;

            db = new ProductDbContext(options);

            List<string?> categoties = await db.Products.Select(p => p.ProductType).Distinct().ToListAsync();

            return categoties;
        }
    }
}
