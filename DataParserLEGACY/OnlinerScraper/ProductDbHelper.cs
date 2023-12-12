using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataParserLEGACY.CustomMappers;
using DataParserLEGACY.DbContext;
using DataParserLEGACY.Models;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;

namespace DataParserLEGACY.OnlinerScraper;

public class ProductDbHelper
{
    public async Task<List<Product>> GetCorrectProductsWithNoRepeatAsync(
        List<Product> productModels)
    {
        var context = EntityFrameworkDbFactory.GetDbContext();


        var existingProductIds = await context.Products.Select(p => p.ParsedUrl).ToListAsync();
        if (existingProductIds == null) throw new ArgumentNullException(nameof(existingProductIds));

        return productModels.Where(p => !existingProductIds.Contains(p.url)).ToList();
    }

    public static double ConvertToDouble(string? s)
    {
        char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
        double result = 0;
        try
        {
            if (s != null)
            {
                result = !s.Contains(",")
                    ? double.Parse(s, CultureInfo.InvariantCulture)
                    : Convert.ToDouble(s.Replace(".", systemSeparator.ToString())
                        .Replace(",", systemSeparator.ToString()));
            }
        }
        catch (Exception)
        {
            try
            {
                result = Convert.ToDouble(s);
            }
            catch
            {
                try
                {
                    result = Convert.ToDouble(s.Replace(",", ";").Replace(".", ",").Replace(";", "."));
                }
                catch
                {
                    throw new Exception("Wrong string-to-double format");
                }
            }
        }

        return result;
    }

    public async Task SendRangeOfDataAsync(List<MainProductModel> RangeofData)
    {
        var db = EntityFrameworkDbFactory.GetDbContext();
        await db.Products.AddRangeAsync(RangeofData);

        await db.SaveChangesAsync();
    }

    public async Task AddProductToDb(MainProductModel product)
    {
        var db = EntityFrameworkDbFactory.GetDbContext();
        await db.Products.AddAsync(product);

        await db.SaveChangesAsync();
    }
}