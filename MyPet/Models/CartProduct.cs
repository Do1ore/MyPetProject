using System.ComponentModel.DataAnnotations.Schema;

namespace MyPet.Models
{
    [Table("CartProduct")]
    public class CartProduct
    {
        public int Id { get; set; }
        [ForeignKey("MainProductModel")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("MainCart")]
        public int CartId { get; set; }
        public MainCart? Cart { get; set; }
        public MainProductModel? ProductModel { get; set; }

    }
}
