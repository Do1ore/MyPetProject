using Microsoft.EntityFrameworkCore;
using MyPet.Models;

namespace DataParserLEGACY.DbContext;

public static class EntityFrameworkDbFactory
{
    public static ProductDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseNpgsql(
                "User ID=postgres;Password=postgre;Host=localhost;Port=5432;Database=MyPet;Pooling=true;")
            .Options;

        return new ProductDbContext(options);
    }
}