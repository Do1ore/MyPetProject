using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels;

namespace MyPet.Areas.SomeLogics
{
    public class ProductHelper
    {
        private static ProductDbContext? db;
        public static async Task<List<string?>> GetAllCategoriesAsync()
        {
            DbContextOptions<ProductDbContext> options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
                .Options;

            db = new ProductDbContext(options);

            List<string?> categoties = await db.Products.Select(p => p.ProductType).Distinct().ToListAsync();

            return categoties;
        }
        public static async Task<IEnumerable<ProductViewModel?>?> GetAllProductsAsync()
        {
            DbContextOptions<ProductDbContext> options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
                .Options;
            AppMappingProfile appMappingProfile = new();

            MapperConfiguration config = new(cfg =>
            {
                _ = cfg.CreateMap<ProductViewModel, MainProductModel>();
            });
            IMapper mapper = config.CreateMapper();
            db = new ProductDbContext(options);
            List<MainProductModel> products = await db.Products.ToListAsync();
            List<ProductViewModel?> productViewModels = new();
            foreach (MainProductModel product in products)
            {
                productViewModels.Add(mapper.Map<ProductViewModel>(product));
            }
            return productViewModels is not null ? productViewModels : (IEnumerable<ProductViewModel?>?)null;
        }

        public static bool CheckFilterForEmptyness(FilterViewModel? filter)
        {
            if (filter is null)
                return true;
            else if (filter.ProductType is null && filter.SortPrice is null && filter.MinPrice is null && filter.MaxPrice is null)
                return true;

            return false;
        }
    }
}
