using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.ViewModels;

namespace MyPet.Controllers
{
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
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyPetUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
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
            }
            return View(model);
        }

        public IActionResult CreateRole() => View();

        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyPetUser user = new MyPetUser {Email = model.Email, UserName = model.Email, FirstName = model.FirstName,
                                                LastName = model.FirstName, RegistrationDateTime = model.RegistrationDateTime};
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
