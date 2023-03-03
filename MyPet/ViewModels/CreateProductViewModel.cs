namespace MyPet.ViewModels
{
    public class CreateProductViewModel
    {
        public int? Rating { get; set; }
        public double? DefaultPrice { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? ProductFullName { get; set; }
        public string? ProductExtendedFullName { get; set; }
        public string? ProductType { get; set; }
        public string? MainFileName { get; set; }
        public string? MainFilePath { get; set; }
    }
}
