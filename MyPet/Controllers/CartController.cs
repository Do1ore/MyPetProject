using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.Models;
using MyPet.ViewModels;
using MyPet.ViewModels.CartProductForAJAX;
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
        private readonly INotyfService notifyService;
        public CartController(ProductDbContext db, UserManager<MyPetUser> userManager, IMapper mapper, MyIdentityDbContext userDb, INotyfService notyfyService)
        {
            this.db = db;
            this.userManager = userManager;
            this.mapper = mapper;
            this.userDb = userDb;
            this.notifyService = notyfyService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart(int id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                notifyService.Error("Пользователь не найден", 10);

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

                // Создаем новый объект CartProduct
                if (cart.CartProducts is null)
                {
                    cart.CartProducts = new List<CartProduct?>();
                }
                var products = await db.Carts
                      .Join(db.CartProducts,
                          cart => cart.Id,
                          cartProd => cartProd.CartId,
                          (cart, cartProd) => new { Cart = cart, CartProduct = cartProd })
                      .Where(joined => joined.Cart.UserId == userId)
                      .AsNoTracking()
                      .Select(joined => joined.CartProduct)
                      .ToListAsync();

                // Добавляем CartProduct в коллекцию CartProducts корзины пользователя
                if (products.Any(i => i.ProductId == id))
                {
                    notifyService.Warning("Товар уже добавлен в корзину", 10);

                    return RedirectToAction("ShowFilteredProduct", "UserProduct");
                }
                CartProduct cartProduct = new() { ProductId = id, Quantity = 1 };

                cart.CartProducts.Add(cartProduct);
                // Сохраняем изменения в базе данных
                await db.SaveChangesAsync();
                notifyService.Success("Успешно добвлено!", 10);
            }
            return RedirectToAction("ShowFilteredProduct", "UserProduct");

        }

        [HttpGet]
        public async Task<IActionResult?> ChosenProducts()
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!db.Carts.Any(cart => cart.UserId == userId))
            {
                return View(new List<ProductAndQuantityViewModel?>());

            }
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
        public async Task<IActionResult> ChangeCartData([FromBody] ProductAndQuantityViewModelForAJAX cartProductViewModel)
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!ModelState.IsValid)
            {
                notifyService.Warning($"Что-то пошло не так");
                return BadRequest(ModelState);
            }
            var products = await db.Carts
            .Join(db.CartProducts,
                cart => cart.Id,
                cartProduct => cartProduct.CartId,
                (cart, cartProduct) => new { Cart = cart, CartProduct = cartProduct })
            .Where(joined => joined.Cart.UserId == userId)
            .Select(joined => joined.CartProduct)
            .ToListAsync();

            foreach (var product in products)
            {
                foreach (var cartProduct in cartProductViewModel.cartProducts)
                {
                    if (product.ProductId == cartProduct.Id)
                    {
                        product.Quantity = cartProduct.Quantity;
                    }
                }
            }
            db.CartProducts.UpdateRange(products);
            await db.SaveChangesAsync();
            notifyService.Information($"Информация обновлена. Товаров изменено: {products.Count}, 10");

            return RedirectToAction(nameof(ChosenProducts));
        }

        [HttpPost]
        public async Task<IActionResult> ExplodeFromCart(int ProductId)
        {
            string? UserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            MyPetUser? user = await userManager.FindByIdAsync(UserId);

            MainCart? cart = await db.Carts.FirstOrDefaultAsync(id => id.UserId == UserId);

            await db.CartProducts.Where(cp => cp.ProductId == ProductId).ExecuteDeleteAsync();
            await db.SaveChangesAsync();
            notifyService.Success("Успешно удалено");
            return RedirectToAction(nameof(ChosenProducts));
        }
        public async Task<IActionResult> ClearCart()
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ICollection<CartProduct?>? productsCarts = await db.Carts.Include(c => c.CartProducts)
               .Where(id => id.UserId == userId)
               .AsNoTracking()
               .Select(p => p.CartProducts)
               .SingleAsync();

            if(productsCarts is null)
            {
                notifyService.Information("Ваша корзина и так пуста");
                return RedirectToAction(nameof(ChosenProducts));
            }

            db.CartProducts.RemoveRange(productsCarts!);
            await db.SaveChangesAsync();
            notifyService.Success($"Корзина товара очищена. Очещено товаров: {productsCarts.Count}");
            return RedirectToAction(nameof(ChosenProducts));

        }

    }
}

