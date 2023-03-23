namespace MyPet.ViewModels
{
    public class ProductsAndFilterViewModel
    {
        public ICollection<ProductViewModel?>? Products { get; set; }
        public FilterViewModel? Filter { get; set; }
    }
}
