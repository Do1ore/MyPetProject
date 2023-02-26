using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyPet.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [Display(Name = "Product name")]
        public string ProductName { get; set; }
        [Required]
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public IFormFile Image { get; set; }
        public string LastTimeEditedString { get; set; }
        public int Price { get; set; }
        public string? Info { get; set; }
    }
}