using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Parsing
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Advertise
    {
        public bool enabled { get; set; }
        public string text { get; set; }
    }

    public class All
    {
        public int term { get; set; }
        public string label { get; set; }
    }

    public class BYN
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Child
    {
        public int id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public string name_prefix { get; set; }
        public string extended_name { get; set; }
        public string status { get; set; }
        public Images images { get; set; }
        public List<object> image_size { get; set; }
        public string description { get; set; }
        public string micro_description { get; set; }
        public string html_url { get; set; }
        public Reviews reviews { get; set; }
        public string review_url { get; set; }
        public string color_code { get; set; }
        public Prices prices { get; set; }
        public MaxInstallmentTerms max_installment_terms { get; set; }
        public MaxCobrandCashback max_cobrand_cashback { get; set; }
        public Sale sale { get; set; }
        public Second second { get; set; }
        public Forum forum { get; set; }
        public string url { get; set; }
        public object advertise { get; set; }
        public List<Sticker> stickers { get; set; }
        public PrimeInfo prime_info { get; set; }
    }

    public class Converted
    {
        public BYN BYN { get; set; }
    }

    public class Forum
    {
        public int? topic_id { get; set; }
        public string topic_url { get; set; }
        public string post_url { get; set; }
        public int? replies_count { get; set; }
    }

    public class Images
    {
        public string header { get; set; }
        public object icon { get; set; }
    }

    public class MaxCobrandCashback
    {
        public int percentage { get; set; }
        public string label { get; set; }
    }

    public class MaxInstallmentTerms
    {
        public All all { get; set; }
    }

    public class MaxPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class MinPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class MinPricesMedian
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Offers
    {
        public int count { get; set; }
    }

    public class Page
    {
        public int limit { get; set; }
        public int items { get; set; }
        public int current { get; set; }
        public int last { get; set; }
    }

    public class PriceMax
    {
        public string amount { get; set; }
        public string currency { get; set; }
        public Converted converted { get; set; }
    }

    public class PriceMin
    {
        public string amount { get; set; }
        public string currency { get; set; }
        public Converted converted { get; set; }
    }

    public class Prices
    {
        public PriceMin price_min { get; set; }
        public PriceMax price_max { get; set; }
        public Offers offers { get; set; }
        public string html_url { get; set; }
        public string url { get; set; }
    }

    public class PrimeInfo
    {
        public bool available { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public string name_prefix { get; set; }
        public string extended_name { get; set; }
        public string status { get; set; }
        public Images images { get; set; }
        public List<object> image_size { get; set; }
        public string description { get; set; }
        public string micro_description { get; set; }
        public string html_url { get; set; }
        public Reviews reviews { get; set; }
        public string review_url { get; set; }
        public string color_code { get; set; }
        public Prices prices { get; set; }
        public MaxInstallmentTerms max_installment_terms { get; set; }
        public MaxCobrandCashback max_cobrand_cashback { get; set; }
        public Sale sale { get; set; }
        public Second second { get; set; }
        public Forum forum { get; set; }
        public string url { get; set; }
        public Advertise advertise { get; set; }
        public List<Sticker> stickers { get; set; }
        public PrimeInfo prime_info { get; set; }
        public List<Child> children { get; set; }
    }

    public class Reviews
    {
        public int rating { get; set; }
        public int count { get; set; }
        public string html_url { get; set; }
        public string url { get; set; }
    }

    public class Root
    {
        public List<Product> products { get; set; }
        public int total { get; set; }
        public Page page { get; set; }
        public int total_ungrouped { get; set; }
    }

    public class Sale
    {
        public bool is_on_sale { get; set; }
        public int discount { get; set; }
        public MinPricesMedian min_prices_median { get; set; }
    }

    public class Second
    {
        public int offers_count { get; set; }
        public MinPrice min_price { get; set; }
        public MaxPrice max_price { get; set; }
    }

    public class Sticker
    {
        public string type { get; set; }
        public string label { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string bg_color { get; set; }
        public object html_url { get; set; }
    }


}
