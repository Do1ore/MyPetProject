using DataParser.Parsing;
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

        private readonly MainProductModel _product;
        private readonly HttpClient _httpClient;
        private HtmlDocument? _htmlDocument;
        private readonly SeleniumDataParser _dataParser;

        public MarketScraper()
        {
            _product = new MainProductModel();
            _httpClient = new HttpClient();
            _htmlDocument = new HtmlDocument();
            _dataParser = new SeleniumDataParser();
        }

        #region Lists
        private readonly List<string> Appointment = new() { "portable", "gaiming", "sport", "swimming" };
        private readonly List<string> HeadphoneType = new() { "headphones with microphone", "wireless headphones with microphone" };
        private readonly List<string> ConnectionType = new() { "wireless", "wired" };
        private readonly List<bool> HasCase = new() { true, false };
        #endregion
        public async Task<MainProductModel?> GenerateHeadphoneAsync(string url)
        {
            Random random = new();
            _htmlDocument = await CreateHtmlDocumentAsync(url);

            #region ProductGeneration
            //start
            string imgsrc = await SelectImg(_htmlDocument);
            string SummaryTitle = await SelectSummaryTitle(_htmlDocument);
            string ShortDescription = await SelectShortDescription(_htmlDocument);
            double ProductPrice = await SelectPrice(_htmlDocument);

            string? headphoneType = HeadphoneType[random.Next(0, HeadphoneType.Count)];
            _ = HasCase[random.Next(0, HasCase.Count)];

            List<string?> ExtraImagesSrc = await SelectSecondaryImg(_htmlDocument);
            List<string?> ExtraFileNames = await CreateExtraFileNameAsync(SummaryTitle, imgsrc, ExtraImagesSrc.Count);
            ICollection<ExtraImageModel> Images = await ProductDbHelper.CreateExtraImagesCollectionAsync(ExtraImagesSrc, ExtraFileNames);

            MainProductModel product = new()
            {
                DefaultPrice = ProductPrice,
                MainFilePath = imgsrc,
                MainFileName = CreateFileName(ShortDescription, imgsrc),
                ShortDescription = ShortDescription,
                ProductType = headphoneType,
                CreationDateTime = DateTime.Now,
                LastTimeEdited = DateTime.Now,
                ParsedUrl = url,
                ExtraImage = Images,
            };
            if (ExtraFileNames is not null && ExtraImagesSrc is not null)
            {
                _ = await ProductDbHelper.CreateExtraImagesCollectionAsync(ExtraImagesSrc, ExtraFileNames);
            }

            #endregion
            return product;
        }

        public async Task<HtmlDocument?> CreateHtmlDocumentAsync(string url)
        {
            string html = await _httpClient.GetStringAsync(url);
            if (_htmlDocument != null)
            {
                _htmlDocument.LoadHtml(html);
            }
            return _htmlDocument;
        }

        public string? CreateFileName(string Title, string src)
        {
            List<string> words = Title.Split(' ').ToList();
            string? FileName = null;
            for (int i = 0; i < words.Count; i++)
            {
                FileName = words.Count >= 2 ? words[0].Trim() + words[1].Trim() + $"Part{i}" : words.First().Trim() + $"Part{i}";
            }
            FileName += DateTime.Now.ToString();
            FileName += Path.GetExtension(src);
            return FileName;
        }




        public async Task<List<string?>> CreateExtraFileNameAsync(string Words, string src, int count)
        {
            List<string?> FileNames = new();
            await Task.Run(() =>
            {
                List<string> words = Words.Split(' ').ToList();
                string Extension = Path.GetExtension(src);
                for (int i = 0; i < count; i++)
                {
                    if (words.Count >= 2)
                    {
                        FileNames.Add(words[0].Trim() + words[1].Trim() + $"Part{i}" + Extension);
                    }
                    else
                    {
                        FileNames.Add(words.First().Trim() + $"Part{i}" + Extension);
                    }
                }
            });
            return FileNames;
        }

        private async Task<string> SelectImg(HtmlDocument htmlDocument)
        {
            string pattern = @"src=\""([^\""]+)\""";
            string? result = null;
            await Task.Run(() =>
            {
                HtmlNode productNode = _htmlDocument.DocumentNode.SelectSingleNode("//div[@class='offers-description__preview']");
                string outerHtml = "";

                outerHtml = productNode.SelectSingleNode(".//img").OuterHtml;
                Match match = Regex.Match(outerHtml, pattern);
                if (match.Success)
                {
                    result = match.Groups[1].Value.Trim();
                }
            });
            return result is not null ? result : throw new NullReferenceException("Null!");
        }

        private async Task<string> SelectSummaryTitle(HtmlDocument htmlDocument)
        {
            string SummaryTitle = "";
            await Task.Run(() =>
            {
                HtmlNode productNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='catalog-masthead__title js-nav-header']");
                SummaryTitle = productNode.InnerHtml.Trim().Replace("\n", "");

            });
            return SummaryTitle.Trim();
        }

        private async Task<string> SelectShortDescription(HtmlDocument htmlDocument)
        {
            string? regex = @"<p>(.*?)</p>";
            string? htmlNode = null;
            string? Result = "";
            await Task.Run(() =>
            {
                HtmlNode productNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='offers-description__specs']");
                htmlNode = productNode.InnerHtml;

                if (htmlNode is not null)
                {
                    Match match = Regex.Match(htmlNode, regex);
                    if (match.Success)
                    {
                        Result = match.Groups[1].Value;
                    }
                }

            });
            return Result.Trim().Replace("\n", "");
        }

        private async Task<double> SelectPrice(HtmlDocument htmlDocument)
        {
            string SummaryTitle = "";
            await Task.Run(() =>
            {
                HtmlNode productNode = htmlDocument.DocumentNode.SelectSingleNode("//a[@class='offers-description__link offers-description__link_nodecor js-description-price-link']");
                SummaryTitle = productNode.InnerHtml;

            });
            return Convert.ToDouble(SummaryTitle.Trim().Replace("\n", "").Replace("&nbsp;р.", ""));
        }

        public async Task<List<string?>> SelectSecondaryImg(HtmlDocument htmlDocument)
        {
            string pattern = @"src=""(.+)""";
            List<string?> ExtraImagesSrc = new();
            await Task.Run(() =>
            {
                HtmlNodeCollection productNodes = _htmlDocument.DocumentNode.SelectNodes("//img[@class='product-gallery__thumb-img']");
                List<string> outerHtml = new();


                foreach (HtmlNode? node in productNodes)
                {
                    if (outerHtml is null)
                    {
                        continue;
                    }

                    outerHtml.Add(node.OuterHtml);
                }
                for (int i = 0; i < outerHtml.Count; i++)
                {
                    Match match = Regex.Match(outerHtml[i], pattern);
                    if (match.Success)
                    {
                        ExtraImagesSrc.Add(match.Groups[1].Value);
                    }
                }


            });
            return ExtraImagesSrc is not null ? ExtraImagesSrc : null;
        }





    }
}
