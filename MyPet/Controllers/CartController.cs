using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.Models;
using MyPet.ViewModels;
using System.Security.Claims;

namespace MyPet.Controllers
{
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
        public async Task<IActionResult> AddProductToCart(int productId, string? UserId)
        {
            MyPetUser? user = await userManager.FindByIdAsync(UserId);

            if (user != null)
            {
                // Получаем корзину пользователя
                MainCart? cart = await db.Carts.FirstOrDefaultAsync(id => id.UserId == UserId);

                if (cart == null)
                {
                    // Если у пользователя еще нет корзины, создаем новую
                    cart = new MainCart();
                    user.MainProductCart = cart;
                }

                // Создаем новый объект CartProduct
                CartProduct cartProduct = new() { ProductId = productId };
                cart.CartProducts ??= new List<CartProduct?>();
                // Добавляем его в коллекцию CartProducts корзины пользователя
                cart.CartProducts.Add(cartProduct);
                // Сохраняем изменения в базе данных
                _ = await db.SaveChangesAsync();
            }
            return RedirectToAction("ShowFilteredProduct", "UserProduct");

        }

        [HttpGet]
        public async Task<IActionResult?> ChosenProducts()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            ICollection<CartProduct?>? productsCarts = await db.Carts.Include(c => c.CartProducts)
                .Where(id => id.UserId == userId)
                .Select(p => p.CartProducts)
                .FirstOrDefaultAsync();

            List<MainProductModel>? products = new();
            foreach (CartProduct? item in productsCarts)
            {
                foreach (MainProductModel? product in await db.Products.ToListAsync())
                {
                    if (product.Id == item.ProductId)
                    {
                        products.Add(product);
                    }
                }
            }

            if (products is not null)
            {
                List<ProductViewModel?> productViewModel = new();
                foreach (MainProductModel item in products)
                {
                    productViewModel.Add(mapper.Map<ProductViewModel?>(item));

                }
                return View(productViewModel);

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ExplodeFromCart(int ProductId, string? UserId)
        {
            MyPetUser? user = await userManager.FindByIdAsync(UserId);

            MainCart? cart = await db.Carts.FirstOrDefaultAsync(id => id.UserId == UserId);

            await db.CartProducts.Where(cp=> cp.ProductId == ProductId).ExecuteDeleteAsync();
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ChosenProducts));
        }

    }
}

