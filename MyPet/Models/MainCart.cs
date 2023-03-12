using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using MyPet.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPet.Models
{
    public class MainCart
    {
        public int Id { get; set; }
        [ForeignKey("MyPetUser")]
        public string? UserId { get; set; }
        public MyPetUser? User { get; set; }
        public ICollection<CartProduct?>? CartProducts { get; set; }
    } 
}
