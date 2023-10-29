using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels;

namespace MyPet.Areas.SomeLogics
{
    public class ProductHelper
    {
        private static ProductDbContext? _db;

        public static async Task<List<string?>> GetAllCategoriesAsync()
        {
            DbContextOptions<ProductDbContext> options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer(
                    "Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
                .Options;

            _db = new ProductDbContext(options);

            List<string?> categoties = await _db.Products.Select(p => p.ProductType).Distinct().ToListAsync();

            return categoties;
        }

       
        public static bool CheckFilterForEmpty(FilterViewModel? filter)
        {
            if (filter is null)
                return true;

            return filter.ProductType is null && filter.SortPrice is null && filter.MinPrice is null &&
                   filter.MaxPrice is null && filter.SearchTerm is null;
        }
    }
}