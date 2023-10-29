using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;

namespace MyPet.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<MyPetUser> _userManager;


        public UserController(MyIdentityDbContext identityDb, UserManager<MyPetUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string Id)
        {
            MyPetUser? user = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(user ?? throw new InvalidOperationException());

            return RedirectToAction(nameof(Index));
        }
    }
}