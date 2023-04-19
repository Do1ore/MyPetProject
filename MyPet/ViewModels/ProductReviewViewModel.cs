using MyPet.Areas.Identity.Data;
using MyPet.Models;

namespace MyPet.ViewModels
{
    public class ProductReviewViewModel
    {
        public int ProductId { get; set; }
        public int ReviewMark { get; set; }
        public string? ReviewText { get; set; }
        public MyPetUser? AppUser { get; set; }
        public DateTime PublishedAt { get; set; }
        public Guid ReviewStorageId { get; set; }
    }
}
