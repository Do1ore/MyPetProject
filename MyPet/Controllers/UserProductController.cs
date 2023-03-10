
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels;
using System.Net;

namespace MyPet.Controllers
{
    public class UserProductController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IMapper mapper;
        private static List<ProductViewModel> productToShow = new();


        public UserProductController(ProductDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IActionResult> SearchProduct(string searchTerm)
        {


            List<ProductViewModel> productViewModels = new();

            var results = await db.Products
            .Where(p => p.ProductFullName.Contains(searchTerm) || p.Description.Contains(searchTerm)
            || p.ProductType.Contains(searchTerm)).ToListAsync();
            List<ProductViewModel> viewmodel = new List<ProductViewModel>();
            if (results.Count > 0)
            {
                foreach (var item in results)
                {
                    productViewModels.Add(mapper.Map<ProductViewModel?>(item));
                }
                ViewBag.Title = $"Товары по запросу: '{searchTerm}'";
                ViewBag.Secondary = $"Товаров: {productViewModels.Count}";
            }
            else
            {
                ViewBag.Title = $"Товары по запросу: '{searchTerm}' не найдены";
                ViewBag.Secondary = $"Вы найти нужный товар во вкладке 'Товары'";

            }
            return View(productViewModels);
        }



        public async Task<IActionResult> Filter(FilterViewModel filter)
        {
            List<MainProductModel> products = await db.Products.ToListAsync();

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowFilteredProduct));
            }

            if (filter.ProductType is not null)
                products = products.Where(p => p.ProductType == filter.ProductType).Select(p => p).ToList();

            if (filter.MinPrice is not null)
                products = products.Where(p => p.DefaultPrice >= filter.MinPrice).Select(p => p).ToList();

            if (filter.MaxPrice is not null)
                products = products.Where(p => p.DefaultPrice <= filter.MaxPrice).Select(p => p).ToList();

            if (filter.SortPrice is not null)
            {
                if (filter.SortPrice == 1)
                    products = products.OrderBy(p => p.DefaultPrice).ToList();
                else if (filter.SortPrice == 2)
                    products = products.OrderByDescending(p => p.DefaultPrice).ToList();
            }

            foreach (var item in products)
            {
                productToShow.Add(mapper.Map<ProductViewModel>(item));
            }
            return RedirectToAction(nameof(ShowFilteredProduct));

        }


        public IActionResult? ShowFilteredProduct()
        {
            var products = productToShow.GetRange(0, productToShow.Count);
            productToShow.Clear();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();
            var products = await db.Products.ToListAsync();
            foreach (var item in products)
            {
                productViewModels.Add(mapper.Map<ProductViewModel>(item));
            }
            return View(productViewModels);
        }

        public async Task<IActionResult?> ViewDetails(int? id)
        {
            ProductDetailedViewModel? productView = new();
            await Task.Run(() =>
            {
                MainProductModel? product = db.Products.Include(i => i.ExtraImage).ToList().Find(i => i.Id == id);
                productView = mapper.Map<ProductDetailedViewModel?>(product);
                if (product is not null)
                {
                    ViewBag.Title = "Детальная информация";
                }
            });
            return View(productView);
        }
    }
}
