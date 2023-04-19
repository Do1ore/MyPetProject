using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.Areas.Services.Abstractions;
using MyPet.Areas.SomeLogics;
using MyPet.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDbContextConnection' not found.");
var config = builder.Configuration;
builder.Services.AddDbContext<MyIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ProductDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddIdentity<MyPetUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<MyIdentityDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.AddScoped<ITimeDifference, TimeDifference>();
// Add roles
builder.Services.AddHealthChecks();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
