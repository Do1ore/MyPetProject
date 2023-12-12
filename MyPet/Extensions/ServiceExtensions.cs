using System.Drawing;
using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.Areas.Services.Abstractions;
using MyPet.Areas.Services.Implementation;
using MyPet.Areas.SomeLogics;
using MyPet.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace MyPet.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityDbContextConnection") ??
                               throw new InvalidOperationException(
                                   "Connection string 'IdentityDbContextConnection' not found.");


        services.AddDbContext<MyIdentityDbContext>(options =>
            options.UseNpgsql(connectionString)
                .EnableSensitiveDataLogging());


        services.AddDbContext<ProductDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .EnableSensitiveDataLogging();
        });

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
        services.AddScoped<ProductHelper>();
        services.AddScoped<NewsManagerService>();
    }

    public static void ConfigureSerilog(IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                          throw new Exception("Variable ASPNETCORE_ENVIRONMENT not found");

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureEls(configuration, environment))
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    private static ElasticsearchSinkOptions ConfigureEls(IConfiguration config, string env)
    {
        var connectionString = config.GetConnectionString("ElasticConnection") ??
                               throw new Exception("Connection for elasticsearch not found");

        return new ElasticsearchSinkOptions(new Uri(connectionString))
        {
            IndexFormat =
                $"{config["ApplicationName"]}-logs-" +
                $"{env.ToLower().Replace(".", "-")}-" +
                $"{DateTime.UtcNow:yyyy-MM)}",
            AutoRegisterTemplate = true,
            NumberOfShards = 2,
            NumberOfReplicas = 1,
        };
    }
}