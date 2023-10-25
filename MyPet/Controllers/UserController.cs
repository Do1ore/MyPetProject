using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;

namespace MyPet.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private UserManager<MyPetUser> userManager;
        private MyIdentityDbContext identityDb;


        public UserController(MyIdentityDbContext identityDb, UserManager<MyPetUser> userManager)
        {
            this.identityDb = identityDb;
            this.userManager = userManager;
        }

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            await userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
