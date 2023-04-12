using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MyPet.Areas.Identity.Data;
using MyPet.Models;
using MyPet.ViewModels;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Security.Claims;

namespace MyPet.Controllers
{
    [Authorize()]
    public class CartController : Controller
    {
        private readonly ProductDbContext db;
        private readonly UserManager<MyPetUser> userManager;
        private readonly MyIdentityDbContext userDb;
        private readonly IMapper mapper;
        public CartController(ProductDbContext db, UserManager<MyPetUser> userManager, IMapper mapper, MyIdentityDbContext users)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
            userDb = users;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart(int id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }
            MyPetUser? user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // Получаем корзину пользователя
                MainCart? cart = await db.Carts.FirstOrDefaultAsync(id => id.UserId == userId);

                if (cart == null)
                {
                    // Если у пользователя еще нет корзины, создаем новую
                    cart = new MainCart();
                    cart.User = user;
                    db.Carts.Add(cart);
                }
                CartProduct cartProduct = new() { ProductId = id, Quantity = 1 };

                // Создаем новый объект CartProduct
                if (cart.CartProducts is null)
                {
                    cart.CartProducts = new List<CartProduct?>();
                }
                // Добавляем его в коллекцию CartProducts корзины пользователя
                cart.CartProducts.Add(cartProduct);
                // Сохраняем изменения в базе данных
                await db.SaveChangesAsync();
            }
            return RedirectToAction("ShowFilteredProduct", "UserProduct");

        }

        [HttpGet]
        public async Task<IActionResult?> ChosenProducts()
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ICollection<CartProduct?>? productsCarts = await db.Carts.Include(c => c.CartProducts)
                .Where(id => id.UserId == userId)
                .AsNoTracking()
                .Select(p => p.CartProducts)
                .SingleAsync();

            if (productsCarts is null)
            {
                return View(new List<ProductAndQuantityViewModel?>());
            }
            IEnumerable<int> ProductIds = productsCarts!.Select(i => i!.ProductId).ToList();


            List<ProductAndQuantityViewModel> productViewModels = new();
            productViewModels = await db.Products.Where(i => ProductIds.Contains(i.Id))
                .Select(p => mapper.Map<ProductAndQuantityViewModel>(p)).ToListAsync();
            //add quantity from cart

            foreach (var cartProduct in productsCarts)
            {
                foreach (var viewModel in productViewModels)
                {
                    if (cartProduct?.ProductId == viewModel.Id)
                    {
                        viewModel.Quatity = cartProduct.Quantity;
                    }
                }

            }
            return View(productViewModels);
        }


        [HttpPost]
        public async Task<IActionResult> ExplodeFromCart(int ProductId, string? UserId)
        {
            MyPetUser? user = await userManager.FindByIdAsync(UserId);

            MainCart? cart = await db.Carts.FirstOrDefaultAsync(id => id.UserId == UserId);

            await db.CartProducts.Where(cp => cp.ProductId == ProductId).ExecuteDeleteAsync();
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ChosenProducts));
        }

    }
}

