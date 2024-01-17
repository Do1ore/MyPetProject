using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.SomeLogics;
using MyPet.Models;
using MyPet.ViewModels.DTOs.News;
using System.Diagnostics;
using Serilog;

namespace MyPet.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsManagerService _newsManagerService;
        private readonly ProductDbContext _db;

        public HomeController(ProductDbContext db, NewsManagerService newsManagerService)
        {
            _db = db;
            _newsManagerService = newsManagerService;
            Log.Debug("Controller {@ControllerName} invoked", nameof(HomeController));
        }

        private static NewsViewModel? NewsViewModel { get; set; }


        public async Task<IActionResult?> Index()
        {
            NewsViewModel = new();
            List<Article?>? news = await _newsManagerService.GetNewsAsync();
            if (news is null)
            {
                NewsViewModel.Articles = new List<Article?>();
            }
            else
            {
                NewsViewModel.Articles = news;
            }

            List<int> ind = await _db.Products.Select(p => p.Id).ToListAsync();
            var randId = new Random().Next(0, ind.Count);

            var product = await _db.Products.Select(p => p)
                .FirstOrDefaultAsync(i => i.Id == ind[randId]);
            if (product != null) ViewBag.Product = product;

            return View(NewsViewModel);
        }

        public async Task<IActionResult> NewsDetails(string title)
        {
            var selectedNews = NewsViewModel?.Articles?.FirstOrDefault(i => i?.Title == title) ?? throw new ArgumentNullException("NewsViewModel?.Articles?.FirstOrDefault(i => i?.Title == title)");
            ViewBag.Title = "Новости";
            ViewBag.Secondary = "Детальная информация";

            return View(selectedNews);
        }

        public IActionResult AllNews()
        {
            ViewBag.Title = "Новости";
            if (NewsViewModel is null)
            {
                ViewBag.Secondary = "Новостей не найдено";
            }
            else
            {
                if (NewsViewModel.Articles != null) ViewBag.Secondary = "Найдено: " + NewsViewModel.Articles.Count;
            }


            return View(NewsViewModel?.Articles);
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