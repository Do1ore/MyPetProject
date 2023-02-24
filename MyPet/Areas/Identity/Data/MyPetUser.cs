using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.AspNetCore.Identity;

namespace MyPet.Areas.Identity.Data;

// Add profile data for application users by adding properties to the MyPetUser class
public class MyPetUser : IdentityUser
{
    
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
    [PersonalData]
    public DateTime RegistrationDateTime { get; set; }
}

