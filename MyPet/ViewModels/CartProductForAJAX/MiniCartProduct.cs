namespace MyPet.ViewModels.CartProductForAJAX
{
    /// <summary>
    /// To accept data from ajax post request in _CartListPartial.cshtml
    /// </summary>
    public class MiniCartProduct
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }


}
