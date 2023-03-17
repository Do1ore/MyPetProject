using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.SomeLogics;
using MyPet.Models;
using MyPet.ViewModels.News;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace MyPet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private NewsManager managerNews;
        private readonly ProductDbContext db;
        public HomeController(ILogger<HomeController> logger, ProductDbContext db)
        {
            _logger = logger;
            this.db = db;
        }
        private static NewsViewModel? newsViewModel { get; set; }


        public async Task<IActionResult?> Index()
        {
            managerNews = new();
            newsViewModel = new();
            newsViewModel.Articles = await managerNews.GetNewsAsync();
            List<int> ind = await db.Products.Select(p => p.Id).ToListAsync();
            var RandId = new Random().Next(0, ind.Count);

            var product = await db.Products.Select(p => p)
                .FirstOrDefaultAsync(i => i.Id == ind[RandId]);
            ViewBag.Product = product;

            return View(newsViewModel);
        }

        public async Task<IActionResult> NewsDetails(string title)
        {
            var SelectedNews = newsViewModel.Articles.FirstOrDefault(i => i.Title == title);
            ViewBag.Title = "Новости";
            ViewBag.Secondary = "Это детальная старница новости " + SelectedNews.Title;

            return View(SelectedNews);
        }

        public IActionResult AllNews()
        {
            ViewBag.Title = "Новости";
            ViewBag.Secondary = "Найдено: " + newsViewModel.Articles.Count;
            return View(newsViewModel.Articles);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}