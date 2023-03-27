using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels.News;
using Newtonsoft.Json;
using RestSharp;

namespace MyPet.Areas.SomeLogics
{
    public class NewsManager : INewsParametres
    {
        private const string NewsID = "9baebdbe994649a59f38eaf8b7bd5e6a";
        private readonly ProductDbContext db;

        public NewsManager()
        {
            DbContextOptions<ProductDbContext> options = new DbContextOptionsBuilder<ProductDbContext>()
               .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
               .Options;

            this.db = new ProductDbContext(options);
        }

        public async Task<List<Article>> GetNewsAsync()
        {
            RestClient client = new("https://newsapi.org/v2/everything");
            NewsViewModel newsViewModel = new();
            List<Article> articles = new();
            NewsApiSettingsModel newsSettings = new();
            RestRequest request = new($"https://newsapi.org/v2/everything", Method.Get);
            request.AddHeader("X-Api-Key", NewsID);

            if (db.NewsApiSettings.Any())
            {
                newsSettings = await db.NewsApiSettings.Select(x => x).FirstAsync();
                if (newsSettings.Sourses is not null)
                {
                    request.AddParameter("sourses", newsSettings.Sourses);
                }
                if (newsSettings.Domains is not null)
                {
                    request.AddParameter("domains", newsSettings.Domains);
                }
                if (newsSettings.DateFrom is not null)
                {
                    request.AddParameter("from", newsSettings.DateFrom.Value.ToString("o"));
                }
                if (newsSettings.DateTo is not null)
                {
                    request.AddParameter("to", newsSettings.DateTo.Value.ToString("o"));
                }
                if (newsSettings.Language is not null)
                {
                    request.AddParameter("language", newsSettings.Language);
                }
                if (newsSettings.PageSize != null || newsSettings.PageSize >= INewsParametres.MaxPageSize || newsSettings.PageSize == 0)
                {
                    request.AddParameter("pageSize", $"{INewsParametres.MaxPageSize}");
                }
                else
                {
                    request.AddParameter("pageSize", $"{newsSettings.PageSize}");

                }
                if (newsSettings.SearchTerm is not null)
                {
                    request.AddParameter("q", $"{newsSettings.SearchTerm}");
                }
                else request.AddParameter("pageSize", $"{INewsParametres.MaxPageSize}");

                RestResponse response = await client.ExecuteAsync(request);
                NewsViewModel? news = JsonConvert.DeserializeObject<NewsViewModel?>(response.Content);
                if (news == null)
                {
                    return new List<Article>();
                }
                newsViewModel.Articles = articles;
                if (articles == null)
                {
                    return new NewsViewModel().Articles;
                }
                foreach (var item in news.Articles)
                {
                    if (item.UrlToImage is not null)
                    {
                        newsViewModel.Articles.Add(item);
                    }
                }
            }
            return newsViewModel.Articles;
        }

        private string JoinList(List<string?> list)
        {
            return string.Join(",", list);
        }
    }
}


