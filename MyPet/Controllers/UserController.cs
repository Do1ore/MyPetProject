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

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
