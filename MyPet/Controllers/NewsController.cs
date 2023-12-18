using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels.DTOs.News;
using Serilog;

namespace MyPet.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewsController : Controller
    {
        private readonly ProductDbContext _db;
        private readonly IMapper _mapper;

        public NewsController(ProductDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            Log.Debug("Controller {@ControllerName} invoked", nameof(NewsController));
        }


        public async Task<IActionResult> ManageNewsSettings()
        {   
            var newSettings = await _db.NewsApiSettings.FirstOrDefaultAsync();
            var settingsViewModel = _mapper.Map<NewsSettingsViewModel>(newSettings);
            return View(settingsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageNewsSettings(NewsSettingsViewModel? newsSettings, string? sources,
            string? domains)
        {
            if (newsSettings is null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (sources is not null)
                newsSettings.Sourses = sources;

            if (domains is not null)
            {
                newsSettings.Domains = domains;
            }

            newsSettings.DateTo ??= DateTime.UtcNow;
            NewsApiSettingsModel? toUpdate = new();
            if (!_db.NewsApiSettings.Any())
                await _db.NewsApiSettings.AddAsync(_mapper.Map<NewsApiSettingsModel>(newsSettings));
            else
            {
                int id = await _db.NewsApiSettings.Select(x => x.Id).FirstAsync();
                toUpdate = _mapper.Map<NewsApiSettingsModel>(newsSettings);
                toUpdate.Id = id;
                _db.NewsApiSettings.Update(toUpdate);
            }

            await _db.SaveChangesAsync();
            Log.Information("News settings was changed: {@NewsSettings}", toUpdate);
            return RedirectToAction("Index", "Home");
        }
    }
}