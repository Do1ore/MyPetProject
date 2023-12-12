using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DataParserLEGACY.SuperDeprecatedStruff.Parsing
{
    public class MarketScraper
    {
        private readonly HttpClient _httpClient = new();
        private HtmlDocument? _htmlDocument = new();
        private readonly SeleniumDataParser _dataParser = new();

        #region Lists

        private readonly List<string> _appointment = new() { "portable", "gaiming", "sport", "swimming" };

        private readonly List<string> _headphoneType = new()
            { "headphones with microphone", "wireless headphones with microphone" };

        private readonly List<string> _connectionType = new() { "wireless", "wired" };
        private readonly List<bool> _hasCase = new() { true, false };

        #endregion


        public async Task<HtmlDocument?> LoadHtmlDocumentAsync(string url)
        {
            try
            {
                string html = await _httpClient.GetStringAsync(url);
                if (_htmlDocument != null)
                {
                    _htmlDocument.LoadHtml(html);
                }

                return _htmlDocument;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            throw new Exception();
        }

        public string? CreateFileName(string Title, string src)
        {
            List<string> words = Title.Split(' ').ToList();
            string? fileName = null;
            for (int i = 0; i < words.Count; i++)
            {
                fileName = words.Count >= 2
                    ? words[0].Trim() + words[1].Trim() + $"Part{i}"
                    : words.First().Trim() + $"Part{i}";
            }

            fileName += DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            fileName += Path.GetExtension(src);
            return fileName;
        }


        public List<string?> CreateExtraFileNameAsync(string words, string src, int count)
        {
            List<string?> fileNames = new();

            List<string> splitWords = words.Split(' ').ToList();
            string extension = Path.GetExtension(src);
            for (int i = 0; i < count; i++)
            {
                if (splitWords.Count >= 2)
                {
                    fileNames.Add(splitWords[0].Trim() + splitWords[1].Trim() + $"Part{i}" + extension);
                }
                else
                {
                    fileNames.Add(splitWords.First().Trim() + $"Part{i}" + extension);
                }
            }

            return fileNames;
        }

        private async Task<string> SelectImg(HtmlDocument htmlDocument)
        {
            string pattern = @"src=\""([^\""]+)\""";
            string? result = null;
            await Task.Run(() =>
            {
                if (_htmlDocument != null)
                {
                    HtmlNode productNode =
                        _htmlDocument.DocumentNode.SelectSingleNode("//div[@class='offers-description__preview']");
                    string outerHtml = "";

                    outerHtml = productNode.SelectSingleNode(".//img").OuterHtml;
                    Match match = Regex.Match(outerHtml, pattern);
                    if (match.Success)
                    {
                        result = match.Groups[1].Value.Trim();
                    }
                }
            });
            return result ?? throw new NullReferenceException("Null!");
        }

        private async Task<string> SelectSummaryTitle(HtmlDocument htmlDocument)
        {
            string summaryTitle = "";
            await Task.Run(() =>
            {
                HtmlNode productNode =
                    htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='catalog-masthead__title js-nav-header']");
                summaryTitle = productNode.InnerHtml.Trim().Replace("\n", "");
            });
            return summaryTitle.Trim();
        }

        private async Task<string> SelectShortDescription(HtmlDocument htmlDocument)
        {
            string? regex = @"<p>(.*?)</p>";
            string? htmlNode = null;
            string? Result = "";
            await Task.Run(() =>
            {
                HtmlNode productNode =
                    htmlDocument.DocumentNode.SelectSingleNode("//div[@class='offers-description__specs']");
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
                HtmlNode productNode = htmlDocument.DocumentNode.SelectSingleNode(
                    "//a[@class='offers-description__link offers-description__link_nodecor js-description-price-link']");
                SummaryTitle = productNode.InnerHtml;
            });
            return Convert.ToDouble(SummaryTitle.Trim().Replace("\n", "").Replace("&nbsp;р.", ""));
        }

        public List<string?> SelectSecondaryImg(HtmlDocument? htmlDocument)
        {
            string pattern = @"src=""(.+)""";
            List<string?> extraImagesSrc = new();

            HtmlNodeCollection productNodes =
                _htmlDocument!.DocumentNode.SelectNodes("//img[@class='product-gallery__thumb-img']");
            List<string> outerHtml = new();


            foreach (HtmlNode? node in productNodes)
            {
                if (outerHtml is null)
                {
                    continue;
                }

                outerHtml.Add(node.OuterHtml);
            }

            if (outerHtml != null)
                for (int i = 0; i < outerHtml.Count; i++)
                {
                    Match match = Regex.Match(outerHtml[i], pattern);
                    if (match.Success)
                    {
                        extraImagesSrc.Add(match.Groups[1].Value);
                    }
                }

            return extraImagesSrc;
        }
    }
}