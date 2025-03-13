// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EternalBAND.Business;
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
    public class ProfileModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _context;
        public ProfileModel(
            UserManager<Users> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Users CurrentUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string? userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var user = userId == null ?
                currentUser :
                _context.Users.Find(userId);
            ViewData["AnotherUserProfileView"] = userId != null && currentUser.Id != userId;
           
            return await OnGetInternalAsync(user);
        }

        private async Task LoadAsync(Users user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            ViewData["PhotoPath"] = user.PhotoPath;
            ViewData["Cities"] = new SelectList(Cities.GetCities(), "Id", "Name", user.City);
            CurrentUser = user;
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
