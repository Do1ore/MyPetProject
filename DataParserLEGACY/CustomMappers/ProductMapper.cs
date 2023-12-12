using System;
using System.Collections.Generic;
using DataParserLEGACY.Models;
using DataParserLEGACY.OnlinerScraper;
using DataParserLEGACY.SuperDeprecatedStruff.Parsing;
using MyPet.Models;

namespace DataParserLEGACY.CustomMappers;

public static class ProductMapper
{
    private static readonly MarketScraper Scraper = new();

    public static MainProductModel MapToMainProductModel(Product t, ICollection<ExtraImageModel> images)
    {
        MainProductModel product = new()
        {
            DefaultPrice =
                Math.Round(
                    (ProductDbHelper.ConvertToDouble(t.prices.price_min.amount) +
                     ProductDbHelper.ConvertToDouble(t.prices.price_min.amount)) /
                    2, 1),
            MinPrice = ProductDbHelper.ConvertToDouble(t.prices.price_min.amount),
            MaxPrice = ProductDbHelper.ConvertToDouble(t.prices.price_max.amount),
            ProductFullName = t.full_name,
            ProductExtendedFullName = t.extended_name,
            MainFilePath = t.images.header,
            MainFileName = Scraper.CreateFileName(t.full_name, t.images.header),
            ShortDescription = t.micro_description,
            Description = t.description,
            ProductType = t.name_prefix,
            Rating = t.reviews.rating,
            CreationDateTime = DateTime.UtcNow,
            LastTimeEdited = DateTime.UtcNow,
            ParsedUrl = t.html_url,
            ExtraImage = images,
        };

        return product;
    }

    public static MainProductModel MapToMainProductModelWithoutImages(Product t)
    {
        MainProductModel product = new()
        {
            DefaultPrice =
                Math.Round(
                    (ProductDbHelper.ConvertToDouble(t.prices.price_min.amount) +
                     ProductDbHelper.ConvertToDouble(t.prices.price_min.amount)) /
                    2, 1),
            MinPrice = ProductDbHelper.ConvertToDouble(t.prices.price_min.amount),
            MaxPrice = ProductDbHelper.ConvertToDouble(t.prices.price_max.amount),
            ProductFullName = t.full_name,
            ProductExtendedFullName = t.extended_name,
            MainFilePath = t.images.header,
            MainFileName = Scraper.CreateFileName(t.full_name, t.images.header),
            ShortDescription = t.micro_description,
            Description = t.description,
            ProductType = t.name_prefix,
            Rating = t.reviews.rating,
            CreationDateTime = DateTime.UtcNow,
            LastTimeEdited = DateTime.UtcNow,
            ParsedUrl = t.html_url
        };

        return product;
    }
}