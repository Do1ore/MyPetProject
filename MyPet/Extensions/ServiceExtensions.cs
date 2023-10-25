using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.Areas.Services.Abstractions;
using MyPet.Areas.SomeLogics;
using MyPet.Models;

namespace MyPet.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityDbContextConnection") ??
                               throw new InvalidOperationException(
                                   "Connection string 'IdentityDbContextConnection' not found.");


        services.AddDbContext<MyIdentityDbContext>(options =>
            options.UseSqlServer(connectionString));


        services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddNotyf(config =>
        {
            config.DurationInSeconds = 10;
            config.IsDismissable = true;
            config.Position = NotyfPosition.TopRight;
        });
        services.AddIdentity<MyPetUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<MyIdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
        
      services.AddScoped<ITimeDifference, TimeDifference>();
    }
}