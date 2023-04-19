﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;
using MyPet.ViewModels.DTOs.News;

namespace MyPet.Controllers
{
    [Authorize(Roles ="admin")]
    public class NewsController : Controller
    {
        private readonly ProductDbContext db;
        private readonly IMapper mapper;
        public NewsController(ProductDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }


        public async Task<IActionResult> ManageNewsSettings()
        {
            var newSettings = await db.NewsApiSettings.FirstAsync();
            var settingsViewModel = mapper.Map<NewsSettingsViewModel>(newSettings);
            return View(settingsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageNewsSettings(NewsSettingsViewModel? newsSettings, string? _sourses, string? _domains)
        {
            if (newsSettings is null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (_sourses is not null)
                newsSettings.Sourses = _sourses;

            if (_domains is not null)
            {
                newsSettings.Domains = _domains;
            }

            if (newsSettings.DateTo is null)
            {
                newsSettings.DateTo = DateTime.Now;
            }
            if(newsSettings.PageSize == null)
            {
                newsSettings.PageSize = 100;
            }
            if (db.NewsApiSettings.Count() == 0)
                await db.NewsApiSettings.AddAsync(mapper.Map<NewsApiSettingsModel>(newsSettings));
            else
            {
                int id = await db.NewsApiSettings.Select(x => x.Id).FirstAsync();
                var toUpdate = mapper.Map<NewsApiSettingsModel>(newsSettings);
                toUpdate.Id = id;
                db.NewsApiSettings.Update(toUpdate);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
