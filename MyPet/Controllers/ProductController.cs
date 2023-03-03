using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels;

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

        public IActionResult ViewDetails(int? id)
        {
            MainProductModel? product = db.Products.Include(i => i.ExtraImage).ToList().Find(i => i.Id == id);
            if (product is not null)
            {
                ViewBag.Title = "Detailed Info";
            }

            return View(product);
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
                    product.LastTimeEdited = DateTime.Now;

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
                .Where(i=> i.ProductModel != null)
                .Where(i => i.ProductModel.Id == product.Id).Select(i => i).ToList();
            db.ExtraImages.RemoveRange(images);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ProductsToList));
        }
        //todo
        public async Task<IActionResult> Create(CreateProductViewModel? product, ExtraImageModel imageModel)
        {
            if(ModelState.IsValid)
            {
                
            }
            return View();
        }
    }
}




