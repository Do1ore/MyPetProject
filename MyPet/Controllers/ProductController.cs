using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.SomeLogics;
using MyPet.Models;
using MyPet.ViewModels;

namespace MyPet.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private static FilterViewModel? buffilter;
        private const int ProductsOnPage = 30;


        public ProductController(ProductDbContext context, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            db = context;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        public async Task<IActionResult> ProductsToList(FilterViewModel filter, int PageNumber)
        {
            if (PageNumber == 0)
            {
                PageNumber = 1;
            }

            ProductsAndFilterViewModel? productsAndFilter = new();
            if (!ProductHelper.CheckFilterForEmpty(filter))
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

            if (ProductHelper.CheckFilterForEmpty(filter) && !ProductHelper.CheckFilterForEmpty(buffilter))
            {
                filter.SortPrice = buffilter.SortPrice;
                filter.MinPrice = buffilter.MinPrice;
                filter.MaxPrice = buffilter.MaxPrice;
                filter.ProductType = buffilter.ProductType;
                filter.SearchTerm = buffilter.SearchTerm;
            }

            List<ProductViewModel?> productViewModel = new();

            List<MainProductModel> products = await db.Products.ToListAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Вы ввели что не так";
                ViewBag.Secondary = "Введите данные в фильтр правильно";
                return View(new ProductsAndFilterViewModel());
            }
            //getting products after filter
            products = FilterProducts(filter, products);
            //mapping MainProductModel to ProductViewModel
            foreach (var item in products)
            {
                productViewModel.Add(_mapper.Map<ProductViewModel>(item));
            }
            //assembling product and filter view model
            List<ProductViewModel> resultProducts = SelectProducts(productViewModel, PageNumber);

            productsAndFilter.Products = resultProducts;
            productsAndFilter.Filter = filter;

            if (productsAndFilter.Products.Count == 0)
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


            int PageCount = (int)Math.Ceiling(productViewModel.Count / (double)ProductsOnPage);


            ViewBag.Quantity = productViewModel.Count;
            ViewBag.ProductsOnPage = PageCount;
            ViewBag.CurrentPage = PageNumber;

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

        private static List<ProductViewModel> SelectProducts(List<ProductViewModel?> products, int CurrentPage)
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

        public IActionResult ViewDetails(int? id)
        {
            MainProductModel? product = db.Products.Include(i => i.ExtraImage).ToList().Find(i => i.Id == id);
            ProductDetailedViewModel? productView = _mapper.Map<ProductDetailedViewModel?>(product);
            if (product is not null)
            {
                ViewBag.Title = "Детальная информация";
            }

            return View(productView);
        }
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, MainProductModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    product.LastTimeEdited = DateTime.UtcNow;

                    db.Update(product);
                    await db.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;
                }
                return RedirectToAction(nameof(ProductsToList));
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();
            var product = await db.Products.FindAsync(id);
            if (product is not null)
                return View(product);
            else return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var extraimages = db.ExtraImages;
            List<ExtraImageModel> images = extraimages
                .Where(i => i.ProductModel != null)
                .Where(i => i.ProductModel.Id == product.Id).Select(i => i).ToList();
            db.ExtraImages.RemoveRange(images);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ProductsToList));
        }
        //todo
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel? product, ExtraImageModel? imageModel, int? ExtaImageCount)
        {
            if (ModelState.IsValid)
            {
                if (imageModel != null && ExtaImageCount is not null)
                    product.ExtraImage.Add(imageModel);
                product.CreationDateTime = DateTime.UtcNow;
                product.LastTimeEdited = DateTime.UtcNow;
                await db.AddAsync(_mapper.Map<MainProductModel>(product));
                await db.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
            return RedirectToAction(nameof(ProductsToList));
        }

        public IActionResult ClearFilter()
        {
            buffilter = new FilterViewModel();
            return RedirectToAction(nameof(ProductsToList));
        }

    }
}




