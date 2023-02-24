using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyPet.Areas.SomeLogics;
using MyPet.Models;
using MyPet.ViewModels;
using System;
using System.Drawing.Text;
using System.Net;

namespace MyPet.Controllers
{
    public class ProductController : Controller
    {
        private ProductDbContext db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(ProductDbContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            _hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> EditProduct(int id, [Bind("Id,ProdcutName,Category,Brand,Description,ShortDescription,FileName,FilePath,Price,Info,CreationDateTime,LastTimeEdited")] ProductModel product, IFormFile image)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

                try
                {
                    if (image != null)
                    {
                        // Delete the old image image if it exists
                        //todo
                        var oldFilePath = product.FilePath;
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }

                        
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string FileName = Path.GetFileNameWithoutExtension(image.FileName);
                        string Extension = Path.GetExtension(image.FileName);
                        FileName = FileName + DateTime.Now.ToString("yymmdd") + Extension;
                        string path = Path.Combine(wwwRootPath + "/img/Products/", FileName);
                        // Save the uploaded image to wwwroot/images folder and update the FilePath property of the product
                        product.FilePath = path;
                        product.FileName = FileName;
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }
                    }

                    product.LastTimeEdited = DateTime.Now;


                    db.Update(product);
                    await db.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;
                }
                return RedirectToAction(nameof(Index));
            
            
        }
    

        public async Task<IActionResult> Index()
        {

            return View(await db.Products.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model, IFormFile image)
        {
            
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string FileName = Path.GetFileNameWithoutExtension(image.FileName);
                string Extension = Path.GetExtension(image.FileName);
                FileName = FileName + DateTime.Now.ToString("yymmdd") + Extension;
                string path = Path.Combine(wwwRootPath + "/img/Products/", FileName);
                
                if (image != null && image.Length > 0) 
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // сохранение изображения в БД
                }
                var product = new ProductModel
                {
                    ProductName = model.ProductName,
                    Description = model.Description,
                    ShortDescription = model.ShortDescription,
                    Price = model.Price,
                    Info = model.Info,
                    Brand = model.Brand,
                    Category = model.Category,
                    FileName = FileName,
                    FilePath = path,
                    CreationDateTime = DateTime.Now.Date,
                    LastTimeEdited = DateTime.Now
                    
                };

                db.Products.Add(product);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            

            
        }
    }
}




