using AspNetCoreHero.ToastNotification.Extensions;
using MyPet.Extensions;
using MyPet.Middlewares;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.ConfigureCustomServices(builder.Configuration);

builder.Services.AddHealthChecks();

builder.WebHost.ConfigureCustomMetrics();

ServiceExtensions.ConfigureSerilog(builder.Configuration);

builder.Host.UseSerilog();
var app = builder.Build();

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
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMetricServer();
app.UseHttpMetrics();
app.Run();
