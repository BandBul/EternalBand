// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EternalBAND.Api;
using EternalBAND.Business;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace EternalBAND.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public IndexModel(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,IWebHostEnvironment hostEnvironment,ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
   

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
               [DisplayName("Ad")]
            [Required(ErrorMessage = "{0} zorunlu.")]
            public string Name { get; set; }

            [DisplayName("Soyad")]
            [Required(ErrorMessage = "{0} zorunlu.")]
            public string Surname { get; set; }
            [DisplayName("Şehir")]
            [Required(ErrorMessage = "{0} zorunlu.")]
            public int City { get; set; }
            [DisplayName("Yaş")]
            [Required(ErrorMessage = "{0} zorunlu.")]
            public int Age { get; set; }
            [Phone]
            [Display(Name = "Telefon Numarası")]
            public string PhoneNumber { get; set; }
            [EmailAddress(ErrorMessage = "Lütfen geçerli bir mail adresi giriniz.")]
            [Display(Name = "Mail Adresi")]
            public string Email { get; set; }
            [Display(Name = "Fotoğrafınız")] public IFormFile Photo { get; set; }
        }

        private async Task LoadAsync(Users user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            ViewData["PhotoPath"] = user.PhotoPath;
            ViewData["Cities"] = new SelectList(Cities.GetCities(), "Id", "Type");
            ViewData["Cities"] = new SelectList(Cities.GetCities(), "Key", "Value", user.City);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Email = userName,
                Name = user.Name,
                Surname = user.Surname,
                City = user.City,
                Age = user.Age
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            if (user.Name != Input.Name || user.Surname != Input.Surname ||user.Age != Input.Age||user.City != Input.City)
            {
                user.Name = Input.Name.ToUpper();
                user.Age = Input.Age;
                user.City = Input.City;
                user.Surname = Input.Surname.ToUpper();
                user.FullName = string.Format("{0} {1}", Input.Name.ToUpper(), Input.Surname.ToUpper());
                await _userManager.UpdateAsync(user);
            }
           
            if (user.Email != Input.Email)
            {
                user.NormalizedEmail = Input.Email.ToUpper();
                user.NormalizedUserName = Input.Email.ToUpper();
                user.UserName = Input.Email;
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.Email);
                var result = await _userManager.ChangeEmailAsync(user, Input.Email, code);
                if (!result.Succeeded)
                {
                    StatusMessage =
                        $"Hata: Bu mail kullanılamaz.";
                   
                    await LoadAsync(user);
                    return Page();
                }
            }
            if (Input.Photo != null)
            {
                try
                {
                    var photoName = string.Format("{0}-{1}{2}", user.FullName.ToLower().Replace(" ", "-") ?? "",
                        user.Id.Replace("-", "").Trim(), System.IO.Path.GetExtension(Input.Photo.FileName));
                    using (var stream =
                           new FileStream(
                               Path.Combine(_hostEnvironment.WebRootPath, "images/user_photo/", photoName),
                               FileMode.Create))
                    {
                        if (!System.IO.Directory.Exists(_hostEnvironment.WebRootPath + "/images/user_photo"))
                        {
                            System.IO.Directory.CreateDirectory(_hostEnvironment.WebRootPath + "/images/user_photo");
                        }

                        await Input.Photo.CopyToAsync(stream);
                        user.PhotoPath = "/images/user_photo/" + photoName;
                    }
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    await new LogService(_context).ErrorLogMethod(
                        "Manage/Index fotoğraf eklenirken hata.", ex,
                        "OnPostAsync");
                }

                
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Telefon numarasını ayarlamaya çalışırken beklenmeyen bir hata oluştu.";
                    return RedirectToPage();
                }
            }

            if(!_context.UserProfileControl.Any(n=> n.UsersId == user.Id && n.Completed))
            {
                _context.UserProfileControl.Add(new UserProfileControl()
                {
                    DateTime = DateTime.Now, Completed = true,
                    UsersId = user.Id
                });
                _context.SaveChanges();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profiliniz güncellendi.";
            return RedirectToPage();
        }
    }
}
