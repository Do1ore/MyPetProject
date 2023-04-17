// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyPet.Areas.Identity.Data;

namespace MyPet.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<MyPetUser> _userManager;
        private readonly SignInManager<MyPetUser> _signInManager;
        private readonly MyIdentityDbContext _identityDbContext;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            UserManager<MyPetUser> userManager,
            SignInManager<MyPetUser> signInManager,
            MyIdentityDbContext identityDbContext,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityDbContext = identityDbContext;
            _environment = environment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        ///
        [Display(Name ="Никнейм")]
        public string Username { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        /// <summary>
        /// Display user image in profile
        /// </summary>
        public string UserProfileImagePath { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Номер телефона")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Изображение профиля")]
            public IFormFile UserProfileImage { get; set; }
        }

        private async Task LoadAsync(MyPetUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var userProfileImagePath = await _identityDbContext.Users.Where(i => i.Id == user.Id)
                .AsNoTracking()
                .Select(p => p.PathToProfileImage)
                .SingleOrDefaultAsync();

            Username = userName;
            UserProfileImagePath = userProfileImagePath;
            Input = new InputModel
            {
                PhoneNumber = phoneNumber

            };
        }

        public async Task<IActionResult> OnGetAsync()
            {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с таким ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Невозможно загрузить пользователя с таким ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.UserProfileImage != null)
            {
                var extension = Path.GetExtension(Input.UserProfileImage.FileName);
                var fileName = "UserProfileImg" + Guid.NewGuid().ToString() + extension;
                var defaultFilepath = Path.Combine("img", "user", "uploads", fileName);

                var filePath = Path.Combine(_environment.WebRootPath, defaultFilepath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.UserProfileImage.CopyToAsync(stream);
                }
                user.PathToProfileImage = @$"/img/user/uploads/{fileName}";
                _identityDbContext.Users.Update(user);
                await _identityDbContext.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Ваш профиль обновлен";
            return RedirectToPage();
        }
    }
}
