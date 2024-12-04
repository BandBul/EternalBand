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
using EternalBAND.Api.Extensions;
namespace EternalBAND.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Users> userManager;
        private readonly IBaseEmailSender emailSender;
        ILogger<ForgotPasswordModel> logger;

        public ForgotPasswordModel(UserManager<Users> userManager, IBaseEmailSender emailSender, ILogger<ForgotPasswordModel> logger)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.logger = logger;
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
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(Input.Email);
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
                    else if (!(await userManager.IsEmailConfirmedAsync(user)))
                    {
                        WarningMessage = $"Henüz e-posta adresinizi doğrulamamış görünüyorsunuz. " +
                            $"Lütfen gelen kutunuzu doğrulama e-postası için kontrol edin veya bu bağlantıyı tıklayarak yeni doğrulama e-postası gönderin.";
                        return Page();
                    }

                    else
                    {
                        var code = await userManager.GeneratePasswordResetTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ResetPassword",
                            pageHandler: null,
                            values: new { area = "Identity", code },
                            protocol: Request.Scheme);

                        await emailSender.SendEmailAsync(
                            Input.Email,
                            "Reset Password",
                            $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Buraya tıklayarak</a> şifrenizi sıfırlayabilirsiniz.");

                        return RedirectToPage("./ForgotPasswordConfirmation");
                    }
                }
                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error happened during ForgotPassword." + ex.Message);
                throw;
            }
           
        }

        private async Task<bool> IsUserMailExternal(Users user)
        {
            var loginData = await userManager.GetLoginsAsync(user);
            return !loginData.IsNullOrEmpty();
        }
    }
}
