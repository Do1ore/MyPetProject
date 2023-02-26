﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyPet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser
{
    public class ProductDbSender
    {

        private static ProductDbContext db;

        public static async Task SendDataAsync(HeadphoneModel model)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
    .UseSqlServer("Server=LAPTOP-CSIKF729;Database=MyPet;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true")
    .Options;

            db = new ProductDbContext(options);
            await db.Headphones.AddAsync(model);
            await db.SaveChangesAsync();
        }
    }
}
