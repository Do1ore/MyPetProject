using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.ViewModels;
using System.Data;

namespace MyPet.Controllers
{
    [Authorize(Roles = "admin")]

    public class AdminController : Controller
    {
        private MyIdentityDbContext db;
        UserManager<MyPetUser> _userManager;
        public AdminController(MyIdentityDbContext context, UserManager<MyPetUser> userManager)
        {
            _userManager = userManager;
            db = context;
        }

            public IActionResult Index()
            {
                return View(db.Users.ToList());
            }

            public async Task<IActionResult> GetRoles()
            {
                db.Add(new IdentityRole("simpleUser"));
                await db.SaveChangesAsync();
                return View(await db.Roles.ToListAsync());
            }

            public async Task<IActionResult> Create(CreateUserViewModel model)
            {
                if (ModelState.IsValid)
                {
                    MyPetUser user = new MyPetUser
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.FirstName,
                        RegistrationDateTime = model.RegistrationDateTime
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return View(model);
            }


    }
}




