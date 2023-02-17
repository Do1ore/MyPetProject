using System.ComponentModel.DataAnnotations;

namespace MyPet.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "������")]
        public string Password { get; set; }

        [Display(Name = "���������?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}