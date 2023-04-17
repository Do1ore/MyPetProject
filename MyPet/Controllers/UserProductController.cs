
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.SomeLogics;
using MyPet.Models;
using MyPet.ViewModels;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace MyPet.Controllers
{
    public sealed class UserProductController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IMapper mapper;
        private static FilterViewModel? buffilter;
        private const int ProductsOnPage = 30;

        public UserProductController(ProductDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IActionResult> SearchProduct(string searchTerm)
        {
            ProductsAndFilterViewModel productAndFilerViewModel = new ProductsAndFilterViewModel();
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
                ViewBag.Title = $"Товар не найден по запросу: '{searchTerm}'";
                ViewBag.Secondary = $"Вы можете найти нужный товар во вкладке 'Товары'";

            }
            FilterViewModel filterViewModel = new FilterViewModel()
            {
                SearchTerm = searchTerm,
            };
            productAndFilerViewModel.Products = productViewModels;
            productAndFilerViewModel.Filter = filterViewModel;
            return View(productAndFilerViewModel);
        }



        public async Task<IActionResult> ShowFilteredProduct(FilterViewModel filter, int pageNumber)
        {
            if (!ProductHelper.CheckFilterForEmptyness(filter))
            {
                buffilter = new FilterViewModel()
                {
                    MaxPrice = filter.MaxPrice,
                    MinPrice = filter.MinPrice,
                    ProductType = filter.ProductType,
                    SortPrice = filter.SortPrice,
                    SearchTerm = filter.SearchTerm,

                };
            }

            if (ProductHelper.CheckFilterForEmptyness(filter) && !ProductHelper.CheckFilterForEmptyness(buffilter))
            {
                filter.SortPrice = buffilter.SortPrice;
                filter.MinPrice = buffilter.MinPrice;
                filter.MaxPrice = buffilter.MaxPrice;
                filter.ProductType = buffilter.ProductType;
                filter.SearchTerm = buffilter.SearchTerm;
            }

            List<ProductViewModel> productToShow = new();

            List<MainProductModel> products = await db.Products.ToListAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Вы ввели что не так";
                ViewBag.Secondary = "Введите данные в фильтр правильно";
                return View(new ProductsAndFilterViewModel());
            }

            products = FilterProducts(filter, products);
            foreach (var item in products)
            {
                productToShow.Add(mapper.Map<ProductViewModel>(item));
            }

            if (productToShow.Count == 0)
            {
                ViewBag.Title = "Товар не найден";
                ViewBag.Secondary = "Попробуйте ввести другие данные в фильтр";

                ViewBag.Quantity = 0;
                ViewBag.ProductsOnPage = 0;
                ViewBag.CurrentPage = 0;
                var result = new ProductsAndFilterViewModel();
                result.Filter = new FilterViewModel();
                result.Products = new List<ProductViewModel?>();
                return View(result);
            }


            int PageCount = (int)Math.Ceiling(productToShow.Count / (double)ProductsOnPage);

            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            List<ProductViewModel> resultProducts = SelectProducts(productToShow, pageNumber);

            ViewBag.Quantity = productToShow.Count;
            ViewBag.ProductsOnPage = PageCount;
            ViewBag.CurrentPage = pageNumber;

            ProductsAndFilterViewModel? productsAndFilter = new();
            productsAndFilter.Products = resultProducts;
            productsAndFilter.Filter = filter;

            return View(productsAndFilter);
        }

        private static List<MainProductModel> FilterProducts(FilterViewModel filter, List<MainProductModel> products)
        {
            if (filter.ProductType is not null && filter.ProductType != "All")
                products = products.Where(p => p.ProductType == filter.ProductType).Select(p => p).ToList();

            if (filter.MinPrice is not null)
                products = products.Where(p => p.DefaultPrice >= filter.MinPrice).Select(p => p).ToList();

            if (filter.MaxPrice is not null)
                products = products.Where(p => p.DefaultPrice <= filter.MaxPrice).Select(p => p).ToList();

            if (filter.SearchTerm is not null)
                products = products.Where(p => p.ProductExtendedFullName.ToUpper().Contains(filter.SearchTerm.ToUpper()) ||
                p.Description.ToUpper().Contains(filter.SearchTerm))
                .Select(p => p).ToList();

            if (filter.SortPrice is not null)
            {
                if (filter.SortPrice == 1)
                    products = products.OrderBy(p => p.DefaultPrice).ToList();
                else if (filter.SortPrice == 2)
                    products = products.OrderByDescending(p => p.DefaultPrice).ToList();
            }

            return products;
        }

        private static List<ProductViewModel> SelectProducts(List<ProductViewModel> products, int CurrentPage)
        {
            int page = CurrentPage - 1;
            List<ProductViewModel> resultProducts = new();
            if (products.Count >= CurrentPage * ProductsOnPage)
            {
                resultProducts = products.GetRange(page * ProductsOnPage, ProductsOnPage);
                return resultProducts;
            }
            else if (page > 0 && products.Count - (page * ProductsOnPage) > 0)
            {
                resultProducts = products.GetRange(page * ProductsOnPage, products.Count - (page * ProductsOnPage));
                return resultProducts;
            }
            return products;
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

        public IActionResult ClearFilter()
        {
            buffilter = new FilterViewModel();
            return RedirectToAction("ShowFilteredProduct");
        }

    }
}
