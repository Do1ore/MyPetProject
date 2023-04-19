using MyPet.Models;

namespace MyPet.ViewModels.DTOs.CartProductForAJAX
{
    /// <summary>
    /// List of data to accept request
    /// </summary>
    public class ProductAndQuantityViewModelForAJAX
    {
        public List<MiniCartProduct> cartProducts { get; set; }
    }
}