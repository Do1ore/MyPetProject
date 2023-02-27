using System.Windows;

namespace DataParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url = @"https://catalog.onliner.by/headphones/dialog/dialep10";
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MarketScraper scraper = new MarketScraper();
            var list = SelenuimDataParser.SelectProductHref("https://catalog.onliner.by/headphones");
            for (int i = 0; i < list.Count; i++)
            {
                var result = await scraper.GenerateHedphoneAsync(list[i]);
                if (result is null)
                {
                    continue;
                }
                await ProductDbHelper.SendDataAsync(result);
            }
            
            MessageBox.Show($"Sucsess. Added {list.Count} products", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
