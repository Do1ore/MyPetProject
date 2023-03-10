using System.ComponentModel.DataAnnotations;

namespace MyPet.ViewModels
{
    public class FilterViewModel
    { 
        public string? ProductType { get; set; }
        public double? MaxPrice { get; set; }
        public double? MinPrice { get; set; }
        public int? SortPrice { get; set; }
    }
}