using System;
using System.Diagnostics;
using System.Windows;
using DataParserLEGACY.Models;
using DataParserLEGACY.OnlinerScraper;

namespace DataParserLEGACY
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


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OnlinerParser parser = new OnlinerParser();
            var input = urltext.Text;

            Root? productModel = await parser.GetMainJsonModelAsync(new Uri(input));

            var productHelper = new ProductDbHelper();
            var res = await productHelper.GetCorrectProductsWithNoRepeatAsync(productModel!.products);
            Debug.WriteLine("With no repeat: " + res.Count);

            var products = parser.GetFullProductsModel(res);

            await foreach (var product in products)
            {
                await productHelper.AddProductToDb(product);
                Debug.WriteLine("Added: " + product);
            }

            MessageBox.Show("Completed!");
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await SuperDeprecatedStruff.SupportingLogics.ProductDbHelper.RemoveDublicates();
        }

        private void textBox1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }
    }
}