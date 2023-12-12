using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DataParserLEGACY.DbContext;
using Microsoft.EntityFrameworkCore;
using MyPet.Models;

namespace DataParserLEGACY.SupportingLogics
{
    public class ProductDbHelper : IAsyncDisposable
    {
        private static ProductDbContext? db;

        private readonly List<char> russianLetters = new List<char>()
        {
            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М',
            'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ',
            'Ы', 'Ь', 'Э', 'Ю', 'Я', 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з',
            'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х',
            'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я'
        };

        private bool _disposed = false;

        // Реализация метода DisposeAsync
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        // Метод для освобождения ресурсов
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (db is not null)
                        await db.DisposeAsync();
                }

                _disposed = true;
            }
        }

        // Деструктор
        ~ProductDbHelper()
        {
            DisposeAsync(false).AsTask().Wait();
        }

        public static async Task SendDataAsync(MainProductModel model)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer(
                    "Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
                .Options;

            db = new ProductDbContext(options);
            db.Add(model);

            await db.SaveChangesAsync();
        }

       

        public static async Task<bool> CheckForRepeatAsync(string url)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer(
                    "Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
                .Options;

            db = new ProductDbContext(options);
            if (await db.Products.AnyAsync(x => x.ParsedUrl == url))
            {
                return true;
            }

            return false;
        }

        public static bool CheckForRepeat(string url)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer(
                    "Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
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

        public static ICollection<ExtraImageModel> CreateExtraImagesCollectionAsync(List<string?> imageSrc,
            List<string?> fileName)
        {

            db = EntityFrameworkDbFactory.GetDbContext();
            
            ICollection<ExtraImageModel> imageModels = new List<ExtraImageModel>();

            ExtraImageModel extraImage;
            if (imageSrc.Count != fileName.Count) throw new Exception("Invalid data");

            for (int i = 0; i < imageSrc.Count; i++)
            {
                extraImage = new ExtraImageModel()
                {
                    FileName = fileName[i],
                    FileSource = imageSrc[i],
                };
                imageModels.Add(extraImage);
            }

            return imageModels;
        }


        public static async Task RemoveDublicates()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseSqlServer(
                    "Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
                .Options;

            db = new ProductDbContext(options);
            int count = await db.Products.CountAsync();
            db.Products.DistinctBy(p => p.ParsedUrl);
            db.SaveChanges();
            int count2 = await db.Products.CountAsync();
            MessageBox.Show("Dublicates removed. Count: {0}", Convert.ToString(count - count2));
        }

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
                    else
                    {
                        counter++;
                    }
            }

            if (counter == strings.Count)
            {
                return false;
            }

            return false;
        }

        private async Task<List<MainProductModel?>> FindModelsAsync(List<int?> idList)
        {
            List<MainProductModel?> products = new List<MainProductModel?>();
            foreach (var key in idList)
            {
                if (await db.Products.AnyAsync(i => i.Id == key))
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