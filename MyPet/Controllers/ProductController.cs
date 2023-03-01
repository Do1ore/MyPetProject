using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;

namespace MyPet.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(ProductDbContext context, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            db = context;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        public async Task<IActionResult> ProductsToList()
        {
            return View(await db.Products.ToListAsync());
        }

        public IActionResult ViewDetails(int id)
        {
            MainProductModel? product = db.Products.Include(i => i.ExtraImage).ToList().Find(i => i.Id == id);
            if (product is not null)
            {
                ViewBag.Title = product.SummaryStroke + " info";
            }

            return View(product);
        }

        #region Legacy
        //public async Task<IActionResult> EditProduct(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await db.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditProduct(int id, ProductModel product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (product.Image != null)
        //            {
        //                // Delete the old image image if it exists
        //                //todo
        //                var oldFilePath = product.FilePath;
        //                if (System.IO.File.Exists(oldFilePath))
        //                {
        //                    System.IO.File.Delete(oldFilePath);
        //                }

        //                string wwwRootPath = _hostEnvironment.WebRootPath;
        //                string FileName = Path.GetFileNameWithoutExtension(product.Image.FileName);
        //                string Extension = Path.GetExtension(product.Image.FileName);
        //                FileName = FileName + DateTime.Now.ToString("yymmdd") + Extension;
        //                string path = Path.Combine(wwwRootPath + "/img/Products/", FileName);
        //                // Save the uploaded image to wwwroot/images folder and update the FilePath property of the product
        //                product.FilePath = path;
        //                product.FileName = FileName;

        //                using (var fileStream = new FileStream(path, FileMode.Create))
        //                {
        //                    await product.Image.CopyToAsync(fileStream);
        //                }
        //            }

        //            product.LastTimeEdited = DateTime.Now;


        //            db.Update(product);
        //            await db.SaveChangesAsync();

        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {

        //            throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);


        //}
        //public async Task<IActionResult> Index()
        //{
        //    return View(await db.Products.ToListAsync());
        //}
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create(ProductViewModel model, IFormFile image)
        //{

        //    string FileName = "";
        //    string Extension = "";
        //    string path = "";
        //    string wwwRootPath = _hostEnvironment.WebRootPath;
        //    if (image is not null)
        //    {
        //        FileName = Path.GetFileNameWithoutExtension(image.FileName);
        //        Extension = Path.GetExtension(image.FileName);
        //        FileName = FileName + DateTime.Now.ToString("yymmdd") + Extension;
        //        path = Path.Combine(wwwRootPath + "/img/Products/", FileName);

        //        if (image != null && image.Length > 0)
        //        {
        //            using (var stream = new FileStream(path, FileMode.Create))
        //            {
        //                await image.CopyToAsync(stream);
        //            }

        //            // сохранение изображения в БД
        //        }
        //    }

        //        var product = _mapper.Map<ProductModel>(model);
        //        product.FileName = FileName;
        //        product.FilePath = path;
        //        db.Products.Add(product);
        //        await db.SaveChangesAsync();

        //        return RedirectToAction("Index", "Home");



        //}
        #endregion
    }
}




