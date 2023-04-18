using System.ComponentModel.DataAnnotations;

namespace MyPet.Models
{
    public class ProductReview
    {
        [Key]
        public Guid ReviewId { get; set; }
        public int ProductId { get; set; }
        public MainProductModel? Product { get; set; }

        public int ReviewMark { get; set; }
        public string? ReviewText { get; set; }

        public DateTime PublishedAt { get; set; }
        public Guid ReviewStorageId { get; set; }
        public ReviewStorage ReviewStorage { get; set; } = new ReviewStorage();
    }
}
