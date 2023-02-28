using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyPet.Models;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace DataParser
{
    public class ProductDbHelper
    {

        private static ProductDbContext db;

        public static async Task SendDataAsync(MainProductModel model)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            await db.Products.AddAsync(model);
            await db.SaveChangesAsync();
        }

        public static async Task<bool?> CheckForRepeatAsync(MainProductModel model, string filePath)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            if (db.Products.Any(x => x.MainFilePath == filePath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async static Task SendExtraImageModelsToDbAsync(List<string?> ImageSrc, List<string?> FileName, MainProductModel productModel)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            int id = await db.Products.CountAsync() + 1;
            ExtraImageModel extraImage;
            if (ImageSrc.Count != FileName.Count) throw new Exception("Invalid data");
           
            await Task.Run(() =>
            {
                for (int i = 0; i < ImageSrc.Count; i++)
                {
                    extraImage = new ExtraImageModel()
                    {
                        ProductId = id,
                        FileName = FileName[i],
                        FileSource = ImageSrc[i],
                    };
                    db.ExtraImages.Add(extraImage);
                }
            });
            await db.SaveChangesAsync();

        }
    }
}
