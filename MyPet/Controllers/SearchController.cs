using Microsoft.AspNetCore.Mvc;
using MyPet.Models;

namespace MyPet.Controllers
{
    public class SearchController : Controller
    {
        private readonly ProductDbContext db;

        public SearchController(ProductDbContext db)
        {
            this.db = db;
        }
        
        public IActionResult FindResult(string searchTerm)
        {
            var results = db.Products
        .Where(p => p.ProductName.Contains(searchTerm) || p.Description.Contains(searchTerm) 
        || p.Brand.Contains(searchTerm) || p.ShortDescription.Contains(searchTerm));

            return View(results.ToList());
        }
    }
}
