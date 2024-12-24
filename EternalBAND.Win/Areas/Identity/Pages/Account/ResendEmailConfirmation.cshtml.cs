#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using EternalBAND.DomainObjects;
using EternalBAND.Api;
using EternalBAND.Common;

namespace EternalBAND.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly IBaseEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<Users> userManager, IBaseEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} zorunlu alandır. Lütfen doldurunuz")]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"{Input.Email} e-posta adresine sahip bir kullanıcı bulunmamaktadır.");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action(
                action: null,
                controller: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme
            );

            callbackUrl = $"{Request.Scheme}://{Request.Host.Value}/{UrlConstants.ConfirmEmail}?{callbackUrl.Split('?')[1]}";

            await _emailSender.SendEmailAsync(
                Input.Email,
                "E-posta adresinizi onaylayın.",
                $"Hesabınızı doğrulamak için lütfen <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>buraya tıklayın</a>.");

            ModelState.AddModelError(string.Empty, "Doğrulama e-postası gönderildi. Lütfen e-posta kutunuzu kontrol edin.");
            return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
        }
    }
}
