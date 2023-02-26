using System.ComponentModel.DataAnnotations.Schema;

namespace MyPet.ViewModels
{
	public class EditProductViewModel
	{
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public int Price { get; set; }
        public string? Info { get; set; }
        public DateTime LastTimeEdited { get; set; }

    }
}
