using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks;
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
        private readonly string NewsID = "9baebdbe994649a59f38eaf8b7bd5e6a";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        private static NewsViewModel? newsViewModel { get; set; }


        public async Task<IActionResult?> Index()
        {
            RestClient client = new("https://newsapi.org/v2/everything");
            //curl https://newsapi.org/v2/everything -G \
            //-d q = Apple \
            //-d from = 2023 - 03 - 14 \
            //-d sortBy = popularity \
            //-d apiKey = 9baebdbe994649a59f38eaf8b7bd5e6a
            //        // client.Authenticator = new HttpBasicAuthenticator(username, password);
            if (newsViewModel is null)
            {
                newsViewModel = new NewsViewModel();
                List<Article> articles = new();
                RestRequest request = new($"https://newsapi.org/v2/everything", Method.Get);
                request.AddHeader("X-Api-Key", $"{NewsID}");
                request.AddParameter("q", "Россия");
                request.AddParameter("language", "ru");
                request.AddParameter("pageSize", "50");
                request.AddParameter("sortBy", "publishedAt");
                RestResponse response = await client.ExecuteAsync(request);

                NewsViewModel? news = JsonConvert.DeserializeObject<NewsViewModel?>(response.Content);
                newsViewModel.Articles = articles;
                foreach (var item in news.Articles)
                {
                    if (item.UrlToImage is not null)
                    {
                        newsViewModel.Articles.Add(item);
                    }
                }
            }
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