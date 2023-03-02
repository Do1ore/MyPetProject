using DataParser.Parsing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyPet.Models;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace DataParser
{
    public class ProductDbHelper
    {

        private static ProductDbContext db;
        private readonly List<char> russianLetters = new List<char>()
        {
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М',
            'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ',
            'Ы', 'Ь', 'Э', 'Ю', 'Я', 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з',
            'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х',
            'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я'
        };

        public static async Task SendDataAsync(MainProductModel model)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            db.Add(model);
            
            await db.SaveChangesAsync();
        }

        public static async Task SendRangeOfDataAsync(List<MainProductModel> RangeofData)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            await db.Products.AddRangeAsync(RangeofData);

            await db.SaveChangesAsync();
        }



        public static async Task<bool> CheckForRepeatAsync(string url)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            if (await db.Products.AnyAsync(x => x.ParsedUrl == url))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool CheckForRepeat(string url)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            if (db.Products.Any(x => x.ParsedUrl == url))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async static Task<ICollection<ExtraImageModel>> CreateExtraImagesCollectionAsync(List<string?> ImageSrc, List<string?> FileName)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;
            db = new ProductDbContext(options);
            ICollection<ExtraImageModel> imageModels = new List<ExtraImageModel>();

            ExtraImageModel extraImage = new();
            if (ImageSrc.Count != FileName.Count) throw new Exception("Invalid data");
            await Task.Run(() =>
            {
                for (int i = 0; i < ImageSrc.Count; i++)
                {
                    extraImage = new ExtraImageModel()
                    {
                        FileName = FileName[i],
                        FileSource = ImageSrc[i],
                    };
                    imageModels.Add(extraImage);
                }
            });
            return imageModels;
        }

        public async Task TranslateDataFromDbAsync()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;
            db = new ProductDbContext(options);

            List<int?> productIdToEdit = new List<int?>();

            List<string?> TranslatedSummary = new();
            List<string?> TranslatedShortDescription = new();

            foreach (var item in db.Products)
            {
                if (item.SummaryStroke.Any(c => russianLetters.Contains(c)))
                {
                    productIdToEdit.Add(item.Id);
                }
                
            }
            List<MainProductModel?> ProductsToEdit = await FindModelsAsync(productIdToEdit);

            if (ProductsToEdit is not null)
            {
                TranslatedSummary = await SelenuimDataParser.TranslateListAsync(await FindSummaryAsync(ProductsToEdit));
                TranslatedShortDescription = await SelenuimDataParser.TranslateListAsync(await FindShortDescriptionAsync(ProductsToEdit));


                for (int i = 0; i < productIdToEdit.Count; i++)
                {

                    ProductsToEdit[i].ShortDescription = TranslatedShortDescription[i];
                    ProductsToEdit[i].SummaryStroke = TranslatedSummary[i];

                    db.Products.Update(ProductsToEdit[i]);

                }
            }
            await db.SaveChangesAsync();
            MessageBox.Show($"Fields translated: {ProductsToEdit.Count}", "Data info", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private async Task<List<MainProductModel?>> FindModelsAsync (List<int?> idList)
        {
             List<MainProductModel?> products = new List<MainProductModel?>();
            foreach (var key in idList)
            {
                if(await db.Products.AnyAsync(i => i.Id==key))
                {
                   products.Add(await db.Products.FindAsync(key));
                }
            }
            return products;
        }

        private async Task<List<string?>> FindSummaryAsync(List<MainProductModel?> products)
        {
            List<string?> values = new List<string?>();
            await Task.Run(() =>
            {

                foreach (var product in products)
                {
                    if (product is not null)
                        values.Add(product.SummaryStroke);
                }
            });
            return values;
        }

        private async Task<List<string?>> FindShortDescriptionAsync(List<MainProductModel?> products)
        {
            List<string?> values = new List<string?>();
            await Task.Run(() =>
            {

                foreach (var product in products)
                {
                    if (product is not null)
                        values.Add(product.ShortDescription);
                }
            });
            return values;
        }

    }
}
