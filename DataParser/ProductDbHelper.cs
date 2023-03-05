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
                return true;
            }
            else
            {
                return false;
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

            List<string?>? TranslatedShortDescription =null;
            List<string?>? TranslatedDescription = null;
            List<string?>? TranslatedExtendedFullName = null;
            List<string?>? TranslatedFullName = null;
            List<string?>? TranslatedProductType = null;


            foreach (var item in db.Products)
            {
                if (item.Description.Any(c => russianLetters.Contains(c) 
                || item.ProductFullName.Any(c => russianLetters.Contains(c))))
                {
                    productIdToEdit.Add(item.Id);
                }
                
            }
            List<MainProductModel?> ProductsToEdit = await FindModelsAsync(productIdToEdit);

            //if (ProductsToEdit is not null)
            //{  

            //    var shortdesc = await FindShortDescriptionAsync(ProductsToEdit);
            //    if(shortdesc is not null)
            //        TranslatedShortDescription = await SeleniumDataParser.TranslateListAsync(shortdesc);

            //    var extendedname = await FindExtenderProductNameAsync(ProductsToEdit);
            //    if(extendedname is not null)
            //    TranslatedExtendedFullName = await SeleniumDataParser.TranslateListAsync(extendedname);

            //    var prodfullname = await FindProductFullNameAsync(ProductsToEdit);
            //    if (prodfullname is not null)
            //        TranslatedExtendedFullName = await SeleniumDataParser.TranslateListAsync(prodfullname);

            //    var description = await FindDescriptionAsync(ProductsToEdit);
            //    if (description is not null)
            //        TranslatedShortDescription = await SeleniumDataParser.TranslateListAsync(description);

            //    var prodtype = await FindProductTypeAsync(ProductsToEdit);
            //    if (prodtype is not null)
            //        TranslatedShortDescription = await SeleniumDataParser.TranslateListAsync(prodtype);
            //    for (int i = 0; i < productIdToEdit.Count; i++)
            //    {

            //        ProductsToEdit[i].ShortDescription = TranslatedShortDescription[i];
            //        ProductsToEdit[i].Description = TranslatedDescription[i];
            //        ProductsToEdit[i].ProductExtendedFullName = TranslatedExtendedFullName[i];
            //        ProductsToEdit[i].ProductType = TranslatedProductType[i];
            //        ProductsToEdit[i].ProductFullName = TranslatedFullName[i];
            //        db.Products.Update(ProductsToEdit[i]);

            //    }
            //}
            if (ProductsToEdit is not null)
            {
                List<List<string?>> superList = new List<List<string?>>();

                var shortdesc = await FindShortDescriptionAsync(ProductsToEdit);
                if (shortdesc is not null)
                    superList.Add(shortdesc);

                var extendedname = await FindExtenderProductNameAsync(ProductsToEdit);
                if (extendedname is not null)
                    superList.Add(extendedname);

                    var prodfullname = await FindProductFullNameAsync(ProductsToEdit);
                if (prodfullname is not null)
                    superList.Add(prodfullname);

                var description = await FindDescriptionAsync(ProductsToEdit);
                if (description is not null)
                    superList.Add(description);

                var prodtype = await FindProductTypeAsync(ProductsToEdit);
                if (prodtype is not null)
                    superList.Add(prodtype);

                List<List<string?>> translatedData = await SeleniumDataParser.TranslateListOfLists(superList);
                for (int i = 0; i < productIdToEdit.Count; i++)
                {

                    ProductsToEdit[i].ShortDescription = translatedData[0][i];
                    ProductsToEdit[i].Description = translatedData[1][i];
                    ProductsToEdit[i].ProductExtendedFullName = translatedData[2][i];
                    ProductsToEdit[i].ProductType = translatedData[3][i];
                    ProductsToEdit[i].ProductFullName = translatedData[4][i];
                    db.Products.Update(ProductsToEdit[i]);

                }
            }
            await db.SaveChangesAsync();
            MessageBox.Show($"Fields translated: {ProductsToEdit.Count}", "Data info", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        //to do translate
        private bool IsContainsRussianLetter(List<string?> strings)
        {
            int counter = 0;
            foreach (var item in strings)
            {
                if (item != null)
                    if (item.Any(c => russianLetters.Contains(c)))
                    {
                        return true;
                    }
                    else { counter++; }
            }
            if(counter == strings.Count)
            {
                return false;
            }
            return false;
            
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
        private async Task<List<string?>?> FindProductFullNameAsync(List<MainProductModel?> products)
        {
            List<string?> values = new List<string?>();

            await Task.Run(() =>
            {

                foreach (var product in products)
                {
                    if (product is not null)
                        values.Add(product.ProductFullName);
                }
            });
            if (!IsContainsRussianLetter(values))
                return null;
            return values;
        }
        private async Task<List<string?>?> FindExtenderProductNameAsync(List<MainProductModel?> products)
        {
            List<string?> values = new List<string?>();
            await Task.Run(() =>
            {

                foreach (var product in products)
                {
                    if (product is not null)
                        values.Add(product.ProductExtendedFullName);
                }
            });
            if (!IsContainsRussianLetter(values))
                return null;
            return values;
        }
        private async Task<List<string?>?> FindProductTypeAsync(List<MainProductModel?> products)
        {
            List<string?> values = new List<string?>();
            await Task.Run(() =>
            {

                foreach (var product in products)
                {
                    if (product is not null)
                        values.Add(product.ProductType);
                }
            });
            if (!IsContainsRussianLetter(values))
                return null;
            return values;
        }
        private async Task<List<string?>?> FindShortDescriptionAsync(List<MainProductModel?> products)
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
            if (!IsContainsRussianLetter(values))
                return null;
            return values;
        }
        private async Task<List<string?>?> FindDescriptionAsync(List<MainProductModel?> products)
        {
            List<string?> values = new List<string?>();
            await Task.Run(() =>
            {

                foreach (var product in products)
                {
                    if (product is not null)
                        values.Add(product.Description);
                }
            });
            if (!IsContainsRussianLetter(values))
                return null;
            return values;
        }

    }
}
