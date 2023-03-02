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

namespace DataParser.Parsing
{
    public class JsonParser
    {
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
        private readonly MarketScraper _scraper;
        private List<MainProductModel> _dataRange;

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

        public async Task HandleHesponse(Root myDeserializedClass)
        {
            ICollection<ExtraImageModel> images = null;
            for (int i = 0; i < myDeserializedClass.products.Count; i++)
            {
                if (myDeserializedClass.products[i].html_url != null)
                {
                    List<string?> ExtraImagesSrc =
                        await _scraper.SelectSecondaryImg(
                        await _scraper.CreateHtmlDocumentAsync(myDeserializedClass.products[i].html_url));
                    List<string?> ExtraFileNames = await _scraper.CreateExtraFileNameAsync(myDeserializedClass.products[i].full_name, myDeserializedClass.products[i].images.header, ExtraImagesSrc.Count);

                    images = await ProductDbHelper.CreateExtraImagesCollectionAsync(ExtraImagesSrc, ExtraFileNames);
                }
                MainProductModel product = new()
                {
                    DefaultPrice = ConvertToDouble((myDeserializedClass.products[i].prices.price_max.amount)),
                    MainFilePath = myDeserializedClass.products[i].images.header,
                    MainFileName = _scraper.CreateFileName(myDeserializedClass.products[i].full_name, myDeserializedClass.products[i].images.header),
                    ShortDescription = myDeserializedClass.products[i].micro_description,
                    Description = myDeserializedClass.products[i].description,
                    SummaryStroke = myDeserializedClass.products[i].full_name,
                    MarketLaunchDate = ProductLaunchDates[new Random().Next(0, ProductLaunchDates.Count)],
                    Appointment = myDeserializedClass.products[i].name_prefix, //to do
                    ProductType = myDeserializedClass.products[i].name_prefix,
                    ConstructionType = ConstructionType[new Random().Next(0, ConstructionType.Count)],
                    ConnectionType = ConnectionType[new Random().Next(0, ConnectionType.Count)],
                    Color = myDeserializedClass.products[i].color_code,
                    BatteryСapacity = BatteryCapacity[new Random().Next(0, BatteryCapacity.Count)] * 10,
                    BluetoothVersion = BluetoothVersion[new Random().Next(0, BluetoothVersion.Count)],
                    MaxRunTime = new Random().Next(2, 5),
                    MaxRunTimeWithCase = new Random().Next(2, 5) * 3,
                    ChargingTime = new Random().Next(2, 5),
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
