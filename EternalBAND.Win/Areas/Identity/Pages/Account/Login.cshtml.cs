#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EternalBAND.DomainObjects;
using EternalBAND.Common;

namespace EternalBAND.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Users> signInManager, ILogger<LoginModel> logger, UserManager<Users> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} zorunlu alandır. Lütfen doldurunuz")]
            [EmailAddress]
            [Display(Name = "Mail Adresi",Prompt ="Mail Adresiniz" )]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} zorunlu alandır. Lütfen doldurunuz")]
            [DataType(DataType.Password)]
            [Display(Name = "Parola",Prompt ="Parolanız")]
            public string Password { get; set; }

            [Display(Name = "Beni hatırla?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Kullanıcı zaten giriş yapmış.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Kullanıcı kilitlendi.");
                    return RedirectToPage("./Lockout");
                }

                if (result.IsNotAllowed)
                {
                    return Redirect($"{EndpointConstants.ErrorRoute}/{(int)ErrorCode.NotConfirmedEmail}");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Hatalı giriş işlemi. Lütfen kullanıcı adınızı ve şifrenizi kontrol ediniz.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
