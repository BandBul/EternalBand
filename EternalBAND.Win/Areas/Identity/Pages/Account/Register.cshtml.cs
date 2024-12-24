#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using EternalBAND.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using EternalBAND.DomainObjects;
using EternalBAND.Api.Attributes;
using EternalBAND.Api;
using System.Globalization;

namespace EternalBAND.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private const string DuplicateUserName = "DuplicateUserName";
        private const string DuplicateEmail = "DuplicateEmail";
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly IUserStore<Users> _userStore;
        private readonly IUserEmailStore<Users> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IBaseEmailSender _emailSender;

        public RegisterModel(
            UserManager<Users> userManager,
            IUserStore<Users> userStore,
            SignInManager<Users> signInManager,
            ILogger<RegisterModel> logger,
            IBaseEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} zorunlu alandır. Lütfen doldurunuz")]
            [EmailAddress]
            [Display(Name = "Mail Adresi",Prompt ="Mail Adresiniz" )]
            
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} zorunlu alandır. Lütfen doldurunuz")]
            [StringLength(100, ErrorMessage = "{0} en az {2} ve en fazla {1} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Parola",Prompt ="Parolanız" )]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Yeniden Parola",Prompt ="Yeniden Parolanız" )]
            [Compare("Password", ErrorMessage = "Parolalar uyuşmuyor.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "KVKK")]
            [MustBeTrue(ErrorMessage = "KVKK ve gizlilik sözleşmesini onaylamadınız")]
            public bool IsPrivacyPolicyAccepted { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.PhotoPath = Constants.DefaultPhotoPath;
                user.RegistrationDate = DateTime.Now;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, Constants.NormalUserRoleName);

                if (result.Succeeded)
                {
                    _logger.LogDebug("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action(
                        action: null,
                        controller: null,
                        values: new { userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme
                    );

                    callbackUrl = $"{Request.Scheme}://{Request.Host.Value}/{UrlConstants.ConfirmEmail}?{callbackUrl.Split('?')[1]}";

                    await _emailSender.SendEmailAsync(Input.Email, "E-posta adresinizi onaylayın.",
                        $"Hesabınızı doğrulamak için lütfen <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>buraya tıklayın</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                var errors = DeleteEmailDuplicationIfExistAndTranslate(result.Errors, Input.Email);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private IEnumerable<IdentityError> DeleteEmailDuplicationIfExistAndTranslate(IEnumerable<IdentityError> errors, string email) 
        {
            var filteredError = errors.Where(s => !s.Code.Equals(DuplicateUserName, StringComparison.InvariantCultureIgnoreCase)).ToList();
            filteredError.ForEach(error =>
            {
                if(error.Code.Equals(DuplicateEmail, StringComparison.InvariantCultureIgnoreCase)) 
                {
                    error.Description = $"'{email}' e-posta adresi zaten kullanılıyor. Lütfen farklı bir e-posta adresi deneyin";
                }
            });

            return filteredError;
        }

        private Users CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Users>();
            }
            catch
            {
                // TODO convert turkish + logging
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Users)}'. " +
                    $"Ensure that '{nameof(Users)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Users> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                // TODO convert turkish + logging
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Users>)_userStore;
        }
    }
}
