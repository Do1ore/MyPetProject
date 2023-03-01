using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPet.Models
{
    public class MainProductModel
    {
        [Key]
        public int Id { get; set; }
        public double Price { get; set; }
        public string? SummaryStroke { get; set; }
        public int? MarketLaunchDate { get; set; }
        public string? Appointment { get; set; }
        public string? ShortDescription { get; set; }
        public string? ConnectionType { get; set; }
        public string? ProductType { get; set; }
        public string? ConstructionType { get; set; }
        public string? Color { get; set; }
        public double? BluetoothVersion { get; set; }
        public double? BatteryСapacity { get; set; }
        public double? ChargingTime { get; set; }
        public double? MaxRunTime { get; set; }
        public double? MaxRunTimeWithCase { get; set; }
        public string? MainFileName { get; set; }
        public string? MainFilePath { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastTimeEdited { get; set; }
        public string? ParsedUrl { get; set; }
        public ICollection<ExtraImageModel> ExtraImage { get; set; }

    }
}
