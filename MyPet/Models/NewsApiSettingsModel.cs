using System.ComponentModel.DataAnnotations;

namespace MyPet.Models
{
    public class NewsApiSettingsModel
    {
        [Key]
        public int Id { get; set; }
        public ICollection<string?>? Sourses { get; set; }
        public ICollection<string?>? Domains { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Language { get; set; }
        public int? PageSize { get; set; }

    }
}
