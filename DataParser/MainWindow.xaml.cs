using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            MarketScraper scraper= new MarketScraper();
            var result = await scraper.GenerateHedphoneAsync(url);
            await ProductDbSender.SendDataAsync(result);
            MessageBox.Show("Sucsess!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
