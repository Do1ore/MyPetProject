﻿using MyPet.Models;
using System.ComponentModel.DataAnnotations;

namespace MyPet.ViewModels
{
    public class CreateProductViewModel
    {
        
        [Display(Name = "Рейтинг")]
        [Range(0, 50, ErrorMessage ="Рейтин может быть только в пределах от {0} до {1}")]
        public int? Rating { get; set; }
        [Display(Name = "Средняя цена")]
        public double? DefaultPrice { get; set; }
        [Display(Name = "Минимальная цена")]
        public double? MinPrice { get; set; }
        [Display(Name = "Максимальная цена")]
        public double? MaxPrice { get; set; }
        [Display(Name = "Короткое описание")]
        public string? ShortDescription { get; set; }
        [Display(Name = "Полное описание")]
        public string? Description { get; set; }
        [Display(Name = "Полное название")]
        public string? ProductFullName { get; set; }
        [Display(Name = "Полное расширенное название")]
        public string? ProductExtendedFullName { get; set; }
        [Display(Name = "Категория")]
        public string? ProductType { get; set; }
        [Display(Name = "Имя основного файла")]
        public string? MainFileName { get; set; }
        [Display(Name = "Основной путь к файлу")]
        public string? MainFilePath { get; set; }
        [Display(Name = "Дата добавления товара")]
        public DateTime? CreationDateTime { get; set; }
        public DateTime? LastTimeEdited { get; set; }
        [Display(Name = "URL страницы")]
        public string? ParsedUrl { get; set; }
        public ICollection<ExtraImageModel?>? ExtraImage { get; set; }

    }
}
