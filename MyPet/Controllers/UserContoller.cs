using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPet.Areas.Identity.Data;
using MyPet.ViewModels;

namespace MyPet.Controllers
{
    //[Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private MyIdentityDbContext db;

        public UserController(MyIdentityDbContext context)
        {
            db = context;

        }

        UserManager<MyPetUser> _userManager;

        

        public IActionResult Index() => View(_userManager.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                MyPetUser user = new MyPetUser { Email = model.Email, UserName = model.Email};
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

       

       

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            MyPetUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}
