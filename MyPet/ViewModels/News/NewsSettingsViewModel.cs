using System.ComponentModel.DataAnnotations;

namespace MyPet.ViewModels.News
{
    public class NewsSettingsViewModel
    {
        public string? Sourses { get; set; }
        public string? Domains { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? Language { get; set; }
        public int PageSize { get; set; }
    }
}
