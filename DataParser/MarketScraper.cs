using HtmlAgilityPack;
using MyPet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataParser
{
    public class MarketScraper
    {

        private readonly HeadphoneModel _headphone;
        private readonly HttpClient _httpClient;
        private readonly HtmlDocument _htmlDocument;
        private readonly SelenuimDataParser _dataParser;
        
        public MarketScraper()
        {
            _headphone = new HeadphoneModel();
            _httpClient = new HttpClient();
            _htmlDocument = new HtmlDocument();
            _dataParser = new SelenuimDataParser();

        }
       
        #region Lists
        List<int> ProductLaunchDates = new List<int>() { 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, DateTime.Now.Year };
        List<string> Appointment = new List<string>() { "portable", "gaiming", "sport", "swimming" };
        List<string> HeadphoneType = new List<string> { "headphones with microphone", "wireless headphones with microphone" };
        List<string> ConstructionType = new List<string>() { "intracanal", "plug-in" };
        List<string> ConnectionType = new List<string>() { "wireless", "wired" };
        List<string> ProdcutColor = new List<string>() { "black", "gray", "white", "yellow", "red", "blue", "orange", "purple", "lemon" };
        List<double> BluetoothVersion = new List<double>() { 4.0, 5.0, 5.2, 4.2, 6.0 };
        List<bool> HasCase = new List<bool>() { true, false };
        List<double> BatteryCapacity = new List<double>() { 30, 40, 45, 50, 24, 25, 10 };
        List<double> CharginngTime = new List<double>() { 1, 1.5, 2, 0.5 };
        List<double> MaxRunTime = new List<double>() { 2, 4, 6, 4.6, 4.7, 5.5, 6.5 };
        #endregion
        public async Task<HeadphoneModel?> GenerateHedphoneAsync(string url)
        {
            Random random = new Random();
     

            var html = await _httpClient.GetStringAsync(url);

            HtmlNodeCollection? productNodes = null;
            _htmlDocument.LoadHtml(html);
            try
            {
                productNodes = _htmlDocument.DocumentNode.SelectNodes("//div[@class='offers-description__preview']");

            }
            catch (Exception)
            {
                return null;
            }
            string outerHtml = "";
            foreach (var productNode in productNodes)
            {
                outerHtml = productNode.SelectSingleNode(".//img").OuterHtml;
            }

            #region ProductGeneration
            //start
            string imgsrc = await SelectImg(_htmlDocument);
            
                string SummaryTitle = await SelectHeader(_htmlDocument);
                string ShortDescription = await SelectShortDescription(_htmlDocument);
                double ProductPrice = await SelectPrice(_htmlDocument);

                string? headphoneType = HeadphoneType[random.Next(0, HeadphoneType.Count)];
                int? productLaunch = ProductLaunchDates[random.Next(0, ProductLaunchDates.Count)];
                string appointment = Appointment[random.Next(0, Appointment.Count)];
                string constructionType = ConstructionType[random.Next(0, ConstructionType.Count)];
                string connectionType = ConnectionType[random.Next(0, ConnectionType.Count)];
                string color = ProdcutColor[random.Next(0, ProdcutColor.Count)];
                double bluetoothVersion = BluetoothVersion[random.Next(0, BluetoothVersion.Count)];
                bool hasCase = HasCase[random.Next(0, HasCase.Count)];
                double batteryCapacity = BatteryCapacity[random.Next(0, BatteryCapacity.Count)];
                double chargingtime = CharginngTime[random.Next(0, CharginngTime.Count)];
                double maxRunTime = MaxRunTime[random.Next(0, MaxRunTime.Count)];
                var product = new HeadphoneModel()
                {
                    Price = ProductPrice,
                    FilePath = imgsrc,
                    FileName = CreateFileName(ShortDescription, imgsrc),
                    ShortDescription = ShortDescription,
                    SummaryStroke = SummaryTitle,
                    MarketLaunchDate = productLaunch,
                    Appointment = appointment,
                    ProductType = headphoneType,
                    ConstructionType = constructionType,
                    ConnectionType = connectionType,
                    Color = color,
                    BatteryСapacity = batteryCapacity,
                    BluetoothVersion = bluetoothVersion,
                    MaxRunTime = maxRunTime,
                    MaxRunTimeWithCase = maxRunTime * 3,
                    ChargingTime = chargingtime,
                    CreationDateTime = DateTime.Now,
                    LastTimeEdited = DateTime.Now,
                };
                #endregion

                return product;
        }

        private string? CreateFileName(string Title, string src)
        {
            var words = Title.Split(' ').ToList();
            int counter = 0;
            string? FileName = null;
            for (int i = 0; i < words.Count; i++)
            {
                if (counter >= 3) break;
                FileName += words[i];
                counter++;
            }
            FileName += DateTime.Now.ToString();
            FileName += Path.GetExtension(src);
            return FileName;
        }
        private async Task<string> SelectImg(HtmlDocument htmlDocument)
        {
            string pattern = @"src=\""([^\""]+)\""";
            string? result = null;
            await Task.Run(() =>
            {
                var productNode = _htmlDocument.DocumentNode.SelectSingleNode("//div[@class='offers-description__preview']");
                string outerHtml = "";

                outerHtml = productNode.SelectSingleNode(".//img").OuterHtml;
                Match match = Regex.Match(outerHtml, pattern);
                if (match.Success)
                    result = match.Groups[1].Value.Trim();

            });
            if (result is not null)
            {
                return result;
            }
            else throw new NullReferenceException("Null!");
        }

        private async Task<string> SelectHeader(HtmlDocument htmlDocument)
        {
            string SummaryTitle = "";
            string result = "";
            await Task.Run(async () =>
            {
                var productNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='catalog-masthead__title js-nav-header']");
                SummaryTitle = productNode.InnerHtml.Trim().Replace("\n", "");
                result = await _dataParser.TranslateData(SummaryTitle);

            });
            return result.Trim();
        }

        private async Task<string> SelectShortDescription(HtmlDocument htmlDocument)
        {
            string? regex = (@"<p>(.*?)</p>");
            string? htmlNode = null;
            string? Result = "";
            await Task.Run(async () =>
            {
                var productNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='offers-description__specs']");
                htmlNode = productNode.InnerHtml;

                if (htmlNode is not null)
                {
                    Match match = Regex.Match(htmlNode, regex);
                    if (match.Success)
                        Result = match.Groups[1].Value;

                }

                Result = await _dataParser.TranslateData(Result);
            });
            return Result.Trim().Replace("\n", "");
        }

        private async Task<double> SelectPrice(HtmlDocument htmlDocument)
        {
            string SummaryTitle = "";
            await Task.Run(() =>
            {
                var productNode = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='offers-description__link offers-description__link_nodecor js-description-price-link']");
                SummaryTitle = productNode.InnerHtml;

            });
            return Convert.ToDouble(SummaryTitle.Trim().Replace("\n", "").Replace("&nbsp;р.", ""));
        }

    
    }
}
