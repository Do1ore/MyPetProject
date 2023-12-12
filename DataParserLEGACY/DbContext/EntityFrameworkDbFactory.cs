using Microsoft.EntityFrameworkCore;
using MyPet.Models;

namespace DataParserLEGACY.DbContext;

public static class EntityFrameworkDbFactory
{
    public static ProductDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlServer(
                "Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
            .Options;

        return new ProductDbContext(options);
    }
}