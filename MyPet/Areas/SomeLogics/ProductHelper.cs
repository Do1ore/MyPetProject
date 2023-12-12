using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels;

namespace MyPet.Areas.SomeLogics
{
    public class ProductHelper
    {
        private readonly ProductDbContext _db;

        public ProductHelper(ProductDbContext db)
        {
            _db = db;
        }

        public async Task<List<string?>> GetAllCategoriesAsync()
        {
   

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