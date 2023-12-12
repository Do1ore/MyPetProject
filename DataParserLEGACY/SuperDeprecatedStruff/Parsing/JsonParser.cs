// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Net.Http;
// using System.Threading;
// using System.Threading.Tasks;
// using System.Windows;
// using DataParserLEGACY.Models;
// using DataParserLEGACY.SupportingLogics;
// using MyPet.Models;
// using Newtonsoft.Json;
//
// namespace DataParserLEGACY.Parsing
// {
//     public class JsonParser : IAsyncDisposable
//     {
//         private readonly bool _disposedValue;
//         public int PagesCount { get; set; }
//         private int Skipped { get; set; }
//         private readonly MarketScraper _scraper;
//         private readonly List<MainProductModel> _dataRange;
//
//         public JsonParser(bool disposedValue)
//         {
//             _disposedValue = disposedValue;
//             _scraper = new MarketScraper();
//             _dataRange = new();
//         }
//
//         private bool _disposed = false;
//
//         public async ValueTask DisposeAsync()
//         {
//             await DisposeAsync(true);
//             GC.SuppressFinalize(this);
//         }
//
//         protected virtual async ValueTask DisposeAsync(bool disposing)
//         {
//             if (!_disposed)
//             {
//                 if (disposing)
//                 {
//                 }
//
//                 _disposed = true;
//             }
//         }
//
//         ~JsonParser()
//         {
//             DisposeAsync(false).AsTask().Wait();
//         }
//
//         public async Task<Root?> GetJsonAsync(Uri url)
//         {
//             Root? myDeserializedClass = new();
//             using (HttpClient httpClient = new())
//             {
//                 string result = await httpClient.GetStringAsync(url);
//                 if (result != null)
//                 {
//                     myDeserializedClass = JsonConvert.DeserializeObject<Root?>(result);
//                 }
//             }
//
//             return myDeserializedClass;
//         }
//
//         public async Task HandleResponse(Root myDeserializedClass)
//         {
//             PagesCount = myDeserializedClass.page.last;
//             foreach (var t in myDeserializedClass.products)
//             {
//                 ICollection<ExtraImageModel>? images;
//                 if (!await ProductDbHelper.CheckForRepeatAsync(t.html_url))
//                 {
//                     List<string?> extraImagesSrc =
//                         _scraper.SelectSecondaryImg(
//                             await _scraper.LoadHtmlDocumentAsync(t.html_url));
//                     List<string?> extraFileNames =
//                         _scraper.CreateExtraFileNameAsync(t.full_name, t.images.header, extraImagesSrc.Count);
//                     images = ProductDbHelper.CreateExtraImagesCollectionAsync(extraImagesSrc, extraFileNames);
//                 }
//                 else
//                 {
//                     Skipped++;
//                     continue;
//                 }
//
//                 MainProductModel product = new()
//                 {
//                     DefaultPrice =
//                         Math.Round(
//                             (ConvertToDouble(t.prices.price_min.amount) + ConvertToDouble(t.prices.price_min.amount)) /
//                             2, 1),
//                     MinPrice = ConvertToDouble(t.prices.price_min.amount),
//                     MaxPrice = ConvertToDouble(t.prices.price_max.amount),
//                     ProductFullName = t.full_name,
//                     ProductExtendedFullName = t.extended_name,
//                     MainFilePath = t.images.header,
//                     MainFileName = _scraper.CreateFileName(t.full_name, t.images.header),
//                     ShortDescription = t.micro_description,
//                     Description = t.description,
//                     ProductType = t.name_prefix,
//                     Rating = t.reviews.rating,
//                     CreationDateTime = DateTime.UtcNow,
//                     LastTimeEdited = DateTime.UtcNow,
//                     ParsedUrl = t.html_url,
//                     ExtraImage = images,
//                 };
//                 _dataRange.Add(product);
//             }
//
//             await ProductDbHelper.SendRangeOfDataAsync(_dataRange);
//             _ = MessageBox.Show($"Success! Added product: {_dataRange.Count}, Scipped: {Skipped}", "Info",
//                 MessageBoxButton.OK, MessageBoxImage.Information);
//         }
//
//         private double ConvertToDouble(string s)
//         {
//             char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
//             double result = 0;
//             try
//             {
//                 if (s != null)
//                 {
//                     result = !s.Contains(",")
//                         ? double.Parse(s, CultureInfo.InvariantCulture)
//                         : Convert.ToDouble(s.Replace(".", systemSeparator.ToString())
//                             .Replace(",", systemSeparator.ToString()));
//                 }
//             }
//             catch (Exception)
//             {
//                 try
//                 {
//                     result = Convert.ToDouble(s);
//                 }
//                 catch
//                 {
//                     try
//                     {
//                         result = Convert.ToDouble(s.Replace(",", ";").Replace(".", ",").Replace(";", "."));
//                     }
//                     catch
//                     {
//                         throw new Exception("Wrong string-to-double format");
//                     }
//                 }
//             }
//
//             return result;
//         }
//     }
// }