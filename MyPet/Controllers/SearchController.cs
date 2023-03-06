using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels;

namespace MyPet.Controllers
{
    public class SearchController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IMapper mapper;
        public SearchController(ProductDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IActionResult> FindResult(string searchTerm)
        {
            var results = await db.Products
        .Where(p => p.ProductFullName.Contains(searchTerm) || p.Description.Contains(searchTerm)
        || p.ProductType.Contains(searchTerm)).ToListAsync();
            List<ProductViewModel> viewmodel = new List<ProductViewModel>();
            
            if (results.Count > 0)
            {
                for (int i =0; i < results.Count; i++)
                {
                    viewmodel.Add(mapper.Map<ProductViewModel>(results[i]));
                    ViewBag.Search = searchTerm;
                }

            }
            return View(viewmodel.ToList());
        }
    }
}
