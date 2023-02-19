using Microsoft.Build.Framework;

public class ProductModel
{
    public int Id { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string ShortDescription { get; set; }
    [Required]
    public string ImageSrc { get; set; }
    public string? Info { get; set; }

}