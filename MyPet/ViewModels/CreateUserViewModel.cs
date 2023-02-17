using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyPet.ViewModels
{
    public class CreateUserViewModel
    {
        [PersonalData]
        [Required]
        public string? FirstName { get; set; }
        [PersonalData]
        [Required]
        public string? LastName { get; set; }
        [PersonalData]
        public string? UserName { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}