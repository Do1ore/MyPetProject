using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using MyPet.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPet.Models
{
    public class ReviewStorage
    {
        public Guid ReviewStorageId { get; set; }
        public MyPetUser? User { get; set; }
        public string? MyPetUserId { get; set; }
        public ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    }
}
