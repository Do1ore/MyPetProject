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
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await using (JsonParser jsonParser = new JsonParser())
            {
                Uri Url = new Uri(urltext.Text);
                var response = await jsonParser.GetJsonAsync(Url);
                textBox1.Text += $"Response accepted. Response product counts: {response.products.Count}\n\r";
                textBox1.Text += $"data in processing... \n\r";
                await Task.WhenAll();
                await jsonParser.HandleResponse(response);
            }

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
