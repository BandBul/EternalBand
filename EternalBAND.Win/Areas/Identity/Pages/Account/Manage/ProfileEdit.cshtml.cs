// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EternalBAND.Business;
using EternalBAND.Common;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EternalBAND.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ProfileEditModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProfileEditModel(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,IWebHostEnvironment hostEnvironment,ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
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
        
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return await OnGetInternalAsync(user);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["AnotherUserProfileView"] = false;
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
            if (user.Name != Input.Name || user.Surname != Input.Surname || user.Age != Input.Age || user.City != Input.City)
            {
                user.Name = Input.Name.ToUpper();
                user.Age = Input.Age;
                user.City = Input.City;
                user.Surname = Input.Surname.ToUpper();
                user.FullName = string.Format("{0} {1}", Input.Name.ToUpper(), Input.Surname.ToUpper());
                await _userManager.UpdateAsync(user);
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
                    // TODO log and decide will it block or not
                }
            }
            else
            {
                user.PhotoPath = Constants.DefaultPhotoPath;
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

            if (!_context.UserProfileControl.Any(n => n.UsersId == user.Id && n.Completed))
            {
                _context.UserProfileControl.Add(new UserProfileControl()
                {
                    DateTime = DateTime.Now,
                    Completed = true,
                    UsersId = user.Id
                });
                _context.SaveChanges();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profiliniz güncellendi.";
            return RedirectToPage("Anasayfa");
        }

        private async Task LoadAsync(Users user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            ViewData["PhotoPath"] = user.PhotoPath;
            ViewData["Cities"] = new SelectList(Cities.GetCities(), "Id", "Name", user.City);

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

        private async Task<IActionResult> OnGetInternalAsync(Users user)
        {
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
    }
}
