using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using DataParserLEGACY.CustomMappers;
using DataParserLEGACY.Models;
using DataParserLEGACY.SuperDeprecatedStruff.Parsing;
using MyPet.Models;
using Newtonsoft.Json;

namespace DataParserLEGACY.OnlinerScraper;

public class OnlinerParser
{
    private readonly MarketScraper _scraper = new();
    public List<MainProductModel>? ProductModels;


    public async Task<Root?> GetMainJsonModelAsync(Uri url)
    {
        Root? myDeserializedClass;
        using (HttpClient httpClient = new())
        {
            string result = await httpClient.GetStringAsync(url);

            myDeserializedClass = JsonConvert.DeserializeObject<Root?>(result);
        }

        return myDeserializedClass;
    }

    public async IAsyncEnumerable<MainProductModel> GetFullProductsModel(List<Product> productDto)
    {
        int completeCounter = 1;
        foreach (var t in productDto)
        {
            await Task.Delay(Random.Shared.Next(213, 442));

            List<string?> extraImagesSrc =
                _scraper!.SelectSecondaryImg(
                    await _scraper.LoadHtmlDocumentAsync(t.html_url));

            List<string?> extraFileNames =
                _scraper.CreateExtraFileNameAsync(t.full_name, t.images.header, extraImagesSrc.Count);

            ICollection<ExtraImageModel>? images =
                SuperDeprecatedStruff.SupportingLogics.ProductDbHelper.CreateExtraImagesCollectionAsync(extraImagesSrc, extraFileNames);

            var product = ProductMapper.MapToMainProductModel(t, images);

            Debug.Print($"Completed [{completeCounter}] / [{productDto.Count}]");

            completeCounter++;

            yield return product;
        }
    }
}