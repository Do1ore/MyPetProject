using MyPet.Models;

namespace MyPet.ViewModels.CartProductForAJAX
{
    /// <summary>
    /// List of data to accept request
    /// </summary>
    public class ProductAndQuantityViewModelForAJAX
    {
        public List<MiniCartProduct> cartProducts { get; set; }
    }
}