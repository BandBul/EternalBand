#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EternalBAND.DomainObjects;
using EternalBAND.Common;
using BandAuthenticationService = EternalBAND.Api.Services.IAuthenticationService;
using EternalBAND.Api.Services;

namespace EternalBAND.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly BandAuthenticationService authenticationService;

        public LoginModel(SignInManager<Users> signInManager, ILogger<LoginModel> logger, UserManager<Users> userManager, BandAuthenticationService authenticationService)
        {
            _signInManager = signInManager;
            _logger = logger;
            this.authenticationService = authenticationService;
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

                var user = await authenticationService.ValidateLogin(Input.Email, Input.Password);

                if (user == null) 
                {
                    ModelState.AddModelError(string.Empty, "Hatalı giriş işlemi. Lütfen kullanıcı adınızı ve şifrenizi kontrol ediniz.");
                    return Page();
                }

                if (!user.EmailConfirmed)
                {
                    //return Forbid("Email not confirmed");
                    return Redirect($"{EndpointConstants.ErrorRoute}/{(int)ErrorCode.NotConfirmedEmail}");
                }

                var token = await authenticationService.CreateTokenAsync(Input.Email);
                Response.Cookies.Append(Constants.AccessTokenCookieName, token, new CookieOptions
                {
                    HttpOnly = false,  // Makes it accessible to JavaScript
                    Secure = true,    // Only sent over HTTPS (for production)
                    SameSite = SameSiteMode.None,  // Prevent CSRF
                });
                return LocalRedirect(returnUrl);
            }

            return Page();
        }
    }
}
