// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using EternalBAND.DomainObjects;
using EternalBAND.Api;
using Microsoft.IdentityModel.Tokens;
namespace EternalBAND.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly IBaseEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<Users> userManager, IBaseEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string WarningMessage { get; set; }



        public class InputModel
        {
            [Required(ErrorMessage = "{0} zorunlu alandır. Lütfen doldurunuz")]
            [EmailAddress]
            [Display(Name = "Mail Adresi",Prompt ="Mail Adresiniz" )]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    WarningMessage = $"'{Input.Email}' mailini kullanan bir kullanıcı bulunmamaktadır.";
                    return Page();
                }

                if (await IsUserMailExternal(user))
                {
                    WarningMessage = "Hesabınız Google ile Oturum Açma kullanıyor. " +
                        "Lütfen giriş yapmak için Google hesabınızı kullanın. Şifre sıfırlamaya gerek yoktur.";
                    return Page();
                }
                //TODO add confirmation email sending logic
                else if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    WarningMessage = $"Henüz e-posta adresinizi doğrulamamış görünüyorsunuz. " +
                        $"Lütfen gelen kutunuzu doğrulama e-postası için kontrol edin veya bu bağlantıyı tıklayarak yeni doğrulama e-postası gönderin.";
                    return Page();
                }

                else 
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Reset Password",
                        $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Buraya tıklayarak</a> şifrenizi sıfırlayabilirsiniz.");

                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
            }
            return Page();
        }

        private async Task<bool> IsUserMailExternal(Users user)
        {
            return !(await _userManager.GetLoginsAsync(user)).IsNullOrEmpty();
        }
    }
}
