using MyPet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPet.ViewModels
{
    public class CartProductViewModel
    {
        public int Quantity { get; set; }
        public MainProductModel? ProductModel { get; set; }
    }
}
