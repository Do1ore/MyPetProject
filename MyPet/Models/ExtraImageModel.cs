using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPet.Models
{
    public class ExtraImageModel
    {
        [Key]
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileSource { get; set; }

        // Foreign key
        [ForeignKey("MainProductModel")]
        public int ProductId { get; set; }
        // Navigation property
        public MainProductModel? ProductModel { get; set; }

    }
}