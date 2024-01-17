using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;
using MyPet.Models;
using MyPet.ViewModels;
using MyPet.ViewModels.DTOs.CartProductForAJAX;
using System.Security.Claims;
using Serilog;

namespace MyPet.Controllers
{
    [Authorize()]
    public class CartController : Controller
    {
        private readonly ProductDbContext _db;
        private readonly UserManager<MyPetUser> _userManager;
        private readonly IMapper _mapper;
        private readonly INotyfService _notifyService;
        public CartController(ProductDbContext db, UserManager<MyPetUser> userManager, IMapper mapper, MyIdentityDbContext userDb, INotyfService notifyService)
        {
           _db = db;
           _userManager = userManager;
           _mapper = mapper;
           _notifyService = notifyService;
           Log.Debug("Controller {@ControllerName} invoked", nameof(CartController));
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart(int id)
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _notifyService.Information("Авторизуйтесь");
                return RedirectToAction("ShowFilteredProduct", "UserProduct");

            }
            var user = await _userManager.FindByIdAsync(userId!);

            if (!_db.Carts.Any(i => i.UserId == userId))
            {
                await _db.Carts.AddAsync(new MainCart { User = user });
                await _db.SaveChangesAsync();
            }

            var cart = await _db.Carts.SingleOrDefaultAsync(i => i.UserId == userId);

            var products = await _db.Carts
                     .Join(_db.CartProducts,
                         cart => cart.Id,
                         cartProd => cartProd.CartId,
                         (cart, cartProd) => new { Cart = cart, CartProduct = cartProd })
                     .Where(joined => joined.Cart.UserId == userId)
                     .AsNoTracking()
                     .Select(joined => joined.CartProduct)
                     .ToListAsync();

            if (products.Any(p => p.ProductId == id))
            {
                _notifyService.Warning("Этот товар уже добавлен в корзину");
                return RedirectToAction("ShowFilteredProduct", "UserProduct");

            }
            CartProduct cartProduct = new CartProduct()
            {
                Cart = cart,
                ProductId = id,
                Quantity = 1,
            };

            await _db.CartProducts.AddAsync(cartProduct);
            await _db.SaveChangesAsync();
            _notifyService.Success("Успешно добвлено!", 10);

            return RedirectToAction("ShowFilteredProduct", "UserProduct");
        }



        [HttpGet]
        public async Task<IActionResult?> ChosenProducts()
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!_db.Carts.Any(cart => cart.UserId == userId))
            {
                return View(new List<ProductAndQuantityViewModel?>());

            }
            
            ICollection<CartProduct?>? productsCarts = await _db.Carts.Include(c => c.CartProducts)
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
            productViewModels = await _db.Products.Where(i => ProductIds.Contains(i.Id))
                .Select(p => _mapper.Map<ProductAndQuantityViewModel>(p)).ToListAsync();
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
                _notifyService.Warning($"Что-то пошло не так");
                return BadRequest(ModelState);
            }
            var products = await _db.Carts
            .Join(_db.CartProducts,
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
            _db.CartProducts.UpdateRange(products);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(ChosenProducts));
        }

        [HttpPost]
        public async Task<IActionResult> ExplodeFromCart(int ProductId)
        {
            string? UserId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _userManager.FindByIdAsync(UserId);

            await _db.Carts.FirstOrDefaultAsync(id => id.UserId == UserId);

            await _db.CartProducts.Where(cp => cp.ProductId == ProductId).ExecuteDeleteAsync();
            await _db.SaveChangesAsync();
            _notifyService.Success("Успешно удалено");
            return RedirectToAction(nameof(ChosenProducts));
        }
        public async Task<IActionResult> ClearCart()
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ICollection<CartProduct?>? productsCarts = await _db.Carts.Include(c => c.CartProducts)
               .Where(id => id.UserId == userId)
               .AsNoTracking()
               .Select(p => p.CartProducts)
               .SingleAsync();

            if (productsCarts is null)
            {
                _notifyService.Information("Ваша корзина и так пуста");
                return RedirectToAction(nameof(ChosenProducts));
            }

            _db.CartProducts.RemoveRange(productsCarts!);
            await _db.SaveChangesAsync();
            _notifyService.Success($"Корзина товара очищена. Очещено товаров: {productsCarts.Count}");
            return RedirectToAction(nameof(ChosenProducts));

        }

    }
}

