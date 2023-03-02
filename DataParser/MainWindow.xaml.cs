using System;
using System.Threading.Tasks;
using System.Windows;
using DataParser.Parsing;

namespace DataParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }
        ProductDbHelper productDbHelper = new ProductDbHelper();
        JsonParser jsonParser = new JsonParser();
        private async void Button_Click(object sender, RoutedEventArgs e)
        {        
            
            Uri Url = new Uri("https://catalog.onliner.by/sdapi/catalog.api/search/hairdryer?page=2");

            var response = await jsonParser.GetJsonAsync(Url);
            await jsonParser.HandleHesponse(response);
            //MarketScraper scraper = new MarketScraper();
            //var url = "https://catalog.onliner.by/headphones";
            

            //var list = await SelenuimDataParser.SelectProductHref(url);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    var result = await scraper.GenerateHeadphoneAsync(list[i]);
            //    if (result is null)
            //    {
            //        continue;
            //    }
            //    await ProductDbHelper.SendDataAsync(result);
            //}

            //MessageBox.Show($"Sucsess. Added {list.Count} products", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await productDbHelper.TranslateDataFromDbAsync();

        }
    }
}
