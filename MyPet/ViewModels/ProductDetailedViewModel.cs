using MyPet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPet.ViewModels
{
    public class ProductDetailedViewModel
    {
        [Key]
        public int Id { get; set; }
        public int? Rating { get; set; }
        public double? DefaultPrice { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? ProductFullName { get; set; }
        public string? ProductExtendedFullName { get; set; }
        public string? ProductType { get; set; }
        public string? MainFileName { get; set; }
        public string? MainFilePath { get; set; }

        public DateTime CreationDateTime { get; set; }
        public DateTime LastTimeEdited { get; set; }
        public string? ParsedUrl { get; set; }
        public ICollection<ExtraImageModel>? ExtraImage { get; set; }
        public ICollection<ProductReview>? Reviews { get; set; }  = new List<ProductReview?>();
    }
}
