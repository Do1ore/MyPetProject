using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using MyPet.Areas.SomeLogics;
using MyPet.Models;

namespace MyPet.Areas.Identity.Data;

// Add profile data for application userDb by adding properties to the MyPetUser class
public class MyPetUser : IdentityUser
{

    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
    [PersonalData]
    public DateTime RegistrationDateTime { get; set; }
    public MainCart? MainProductCart { get; set; }
    [NotMapped]
    public IFormFile? UserProfileImage { get; set; }
    public string? PathToProfileImage { get; set; } = DefaultUserImage.ImgUrl;
}

