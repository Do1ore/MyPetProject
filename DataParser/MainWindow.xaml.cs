using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using DataParser.Parsing;
using DataParser.SupportingLogics;

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
            
            Uri Url = new Uri(urltext.Text);
            var response = await jsonParser.GetJsonAsync(Url);
            textBox1.Text += $"Response accepted. Response product counts: {response.products.Count}\n\r";
            textBox1.Text += $"data in processing... \n\r";
            await jsonParser.HandleResponse(response);

            //MarketScraper scraper = new MarketScraper();
            //var url = "https://catalog.onliner.by/headphones";


            //var list = await SeleniumDataParser.SelectProductHref(url);
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
            await ProductDbHelper.RemoveDublicates();

        }

        private void textBox1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
