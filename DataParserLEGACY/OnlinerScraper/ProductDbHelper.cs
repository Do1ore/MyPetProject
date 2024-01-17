using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        if (existingProductIds?.Count == 0) return productModels;

        return productModels.Where(p => existingProductIds != null && !existingProductIds.Contains(p.url)).ToList();
    }

    public static double ConvertToDouble(string? stringNumber)
    {
        char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
        double result = 0;
        try
        {
            if (stringNumber != null)
            {
                result = !stringNumber.Contains(",")
                    ? double.Parse(stringNumber, CultureInfo.InvariantCulture)
                    : Convert.ToDouble(stringNumber.Replace(".", systemSeparator.ToString())
                        .Replace(",", systemSeparator.ToString()));
            }
        }
        catch (Exception)
        {
            try
            {
                result = Convert.ToDouble(stringNumber);
            }
            catch
            {
                try
                {
                    result = Convert.ToDouble(stringNumber?
                        .Replace(",", ";")
                        .Replace(".", ",")
                        .Replace(";", "."));
                }
                catch
                {
                    throw new Exception("Wrong string-to-double format");
                }
            }
        }

        return result;
    }

    public async Task SendRangeOfDataAsync(List<MainProductModel> rangeOfData)
    {
        var db = EntityFrameworkDbFactory.GetDbContext();
        await db.Products.AddRangeAsync(rangeOfData);

        await db.SaveChangesAsync();
    }

    public async Task AddProductToDb(MainProductModel product)
    {
        var db = EntityFrameworkDbFactory.GetDbContext();
        await db.Products.AddAsync(product);

        await db.SaveChangesAsync();
    }
}