using HtmlAgilityPack;
using Microsoft.DotNet.MSIdentity.Shared;
using MyPet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DataParser.Parsing
{
    public class JsonParser
    {
        
        public int PagesCount { get; set; }
        public int Scipped { get; set; }
        private readonly MarketScraper _scraper;
        private List<MainProductModel> _dataRange;
        #region Lists
        private readonly List<int> ProductLaunchDates = new() { 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, DateTime.Now.Year };
        private readonly List<string> Appointment = new() { "portable", "gaiming", "sport", "swimming" };
        private readonly List<string> HeadphoneType = new() { "headphones with microphone", "wireless headphones with microphone" };
        private readonly List<string> ConstructionType = new() { "intracanal", "plug-in" };
        private readonly List<string> ConnectionType = new() { "wireless", "wired" };
        private readonly List<string> ProdcutColor = new() { "black", "gray", "white", "yellow", "red", "blue", "orange", "purple", "lemon" };
        private readonly List<double> BluetoothVersion = new() { 4.0, 5.0, 5.2, 4.2, 6.0 };
        private readonly List<bool> HasCase = new() { true, false };
        private readonly List<double> BatteryCapacity = new() { 30, 40, 45, 50, 24, 25, 10 };
        private readonly List<double> CharginngTime = new() { 1, 1.5, 2, 0.5 };
        private readonly List<double> MaxRunTime = new() { 2, 4, 6, 4.6, 4.7, 5.5, 6.5 };
        #endregion
        public JsonParser()
        {
            _scraper = new MarketScraper();
            _dataRange = new();
    }

        public async Task<Root?> GetJsonAsync(Uri url)
        {
            Root? myDeserializedClass = new Root();
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.GetStringAsync(url);
                if (result != null)
                {
                    myDeserializedClass = JsonConvert.DeserializeObject<Root?>(result);
                }
            }
            return myDeserializedClass;
        }

        public async Task HandleResponse(Root myDeserializedClass)
        {
            PagesCount = myDeserializedClass.page.last;
            ICollection<ExtraImageModel> images = null;
            for (int i = 0; i < myDeserializedClass.products.Count; i++)
            {
                if (myDeserializedClass.products[i].html_url != null &&  !(await ProductDbHelper.CheckForRepeatAsync(myDeserializedClass.products[i].html_url)))
                {
                    List<string?> ExtraImagesSrc =
                        await _scraper.SelectSecondaryImg(
                        await _scraper.CreateHtmlDocumentAsync(myDeserializedClass.products[i].html_url));
                    List<string?> ExtraFileNames = await _scraper.CreateExtraFileNameAsync(myDeserializedClass.products[i].full_name, myDeserializedClass.products[i].images.header, ExtraImagesSrc.Count);

                    images = await ProductDbHelper.CreateExtraImagesCollectionAsync(ExtraImagesSrc, ExtraFileNames);
                }
                else
                {
                    Scipped++;
                    continue;
                }
                MainProductModel product = new()
                {
                    DefaultPrice = Math.Round((ConvertToDouble(myDeserializedClass.products[i].prices.price_min.amount) + ConvertToDouble(myDeserializedClass.products[i].prices.price_min.amount))/2, 1),
                    MinPrice = ConvertToDouble(myDeserializedClass.products[i].prices.price_min.amount),
                    MaxPrice = ConvertToDouble(myDeserializedClass.products[i].prices.price_max.amount),
                    ProductFullName = myDeserializedClass.products[i].full_name,
                    ProductExtendedFullName = myDeserializedClass.products[i].extended_name,
                    MainFilePath = myDeserializedClass.products[i].images.header,
                    MainFileName = _scraper.CreateFileName(myDeserializedClass.products[i].full_name, myDeserializedClass.products[i].images.header),
                    ShortDescription = myDeserializedClass.products[i].micro_description,
                    Description = myDeserializedClass.products[i].description,
                    ProductType = myDeserializedClass.products[i].name_prefix,
                    Rating = myDeserializedClass.products[i].reviews.rating,
                    CreationDateTime = DateTime.Now,
                    LastTimeEdited = DateTime.Now,
                    ParsedUrl = myDeserializedClass.products[i].html_url,
                    ExtraImage = images,
                };
                _dataRange.Add(product);
            }
            if (_dataRange is not null)
            {
                await ProductDbHelper.SendRangeOfDataAsync(_dataRange);
            }
            MessageBox.Show($"Success! Added product: {_dataRange.Count}, Scipped: {Scipped}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private double ConvertToDouble(string s)
        {
            char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            double result = 0;
            try
            {
                if (s != null)
                    if (!s.Contains(","))
                        result = double.Parse(s, CultureInfo.InvariantCulture);
                    else
                        result = Convert.ToDouble(s.Replace(".", systemSeparator.ToString()).Replace(",", systemSeparator.ToString()));
            }
            catch (Exception e)
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

    }
}
