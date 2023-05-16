using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.Areas.SomeLogics;
using MyPet.Models;
using MyPet.ViewModels;
using MyPet.ViewModels.DTOs;
using System.Security.Claims;

namespace MyPet.Controllers
{
    public sealed class UserProductController : Controller
    {
        private readonly ProductDbContext db;
        private readonly UserManager<MyPetUser> userManager;
        private readonly IMapper mapper;
        private readonly INotyfService notify;
        private static FilterViewModel? buffilter;
        private const int ProductsOnPage = 30;

        public UserProductController(ProductDbContext db, IMapper mapper, INotyfService notify, UserManager<MyPetUser> userManager)
        {
            this.db = db;
            this.mapper = mapper;
            this.notify = notify;
            this.userManager = userManager;
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

            var product = await db.Products.Include(u => u.ExtraImage)
                                            .AsNoTracking()
                                            .Where(p => p.Id == id)
                                            .SingleOrDefaultAsync();

            productView = mapper.Map<ProductDetailedViewModel>(product);
            if (product is not null)
            {
                ViewBag.Title = "Детальная информация";
            }

            //get reviews for current product
            var reviews = await db.ReviewStorages.Join(db.ProductReviews,
                                storage => storage.ReviewStorageId,
                                review => review.ReviewStorageId,
                                (joined, joinedR) => new { Reviews = joinedR, Storage = joined })
                                .Where(p => p.Reviews.ProductId == id)
                                .AsNoTracking()
                                .Select(u => u.Reviews)
                                .ToListAsync();

            var StorageIds = reviews.Select(i => i.ReviewStorageId).ToList();

            List<ProductReviewViewModel> reviewViewModel = new List<ProductReviewViewModel>();

            foreach (var item in reviews)
            {
                reviewViewModel.Add(mapper.Map<ProductReviewViewModel>(item));
            }
            var fullReviewViewModel = await GetFullViewModelForReviewAsync(reviewViewModel, StorageIds);

            if (reviews is not null)
                productView.Reviews = fullReviewViewModel;

            return View(productView);
        }

        public IActionResult ClearFilter()
        {
            buffilter = new FilterViewModel();
            return RedirectToAction("ShowFilteredProduct");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendReview([FromBody] ReviewDTO review)
        {
            bool StorageNotCreated = false;
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!ModelState.IsValid)
            {
                notify.Warning("Форма не правильно заполнена");
                return RedirectToAction(nameof(ViewDetails), "UserProduct", new { id = review.ProductId });

            }

            if (!db.ReviewStorages.Any(i => i.MyPetUserId == userId))
            {
                await db.ReviewStorages.AddAsync(new ReviewStorage { MyPetUserId = userId });
                await db.SaveChangesAsync();
            }

            var reviewStorage = await db.ReviewStorages.SingleOrDefaultAsync(i => i.MyPetUserId == userId);

            ProductReview productReview = new()
            {
                PublishedAt = DateTime.Now,
                ReviewStorageId = reviewStorage!.ReviewStorageId,
                ProductId = review.ProductId,
                ReviewMark = (int)review.Rating!,
                ReviewText = review.Text,
            };
            reviewStorage.ProductReviews.Add(productReview);
            await db.SaveChangesAsync();
            await UpdateProductRatingAsync(review.ProductId);
            return new JsonResult(new { id = review.ProductId, success = true });
        }

        private async Task UpdateProductRatingAsync(int productId)
        {
            var reviews = await db.ProductReviews
                .Where(i => i.ProductId == productId)
                .Select(a => a.ReviewMark).ToListAsync();

            await db.Products
                .Where(i => i.Id == productId)
                .ExecuteUpdateAsync(p => p.SetProperty(a => a.Rating, reviews.Sum() / reviews.Count));
        }

        private async Task<ICollection<ProductReviewViewModel>> GetFullViewModelForReviewAsync(List<ProductReviewViewModel> productReviews, List<Guid> ids)
        {
            var storages = await db.ReviewStorages.Where(i => ids.Contains(i.ReviewStorageId))
                                              .Select(a => a)
                                              .ToListAsync();
            foreach (var storage in storages)
            {
                foreach (var review in productReviews)
                {
                    if (review.ReviewStorageId == storage.ReviewStorageId)
                    {
                        review.AppUser = await userManager.FindByIdAsync(storage.MyPetUserId);
                    }
                }
            }

            return productReviews;
        }

    }
}
