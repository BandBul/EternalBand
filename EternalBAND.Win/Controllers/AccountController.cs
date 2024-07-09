using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EternalBAND.Api.Services;

namespace EternalBAND.Controllers
{
    [AllowAnonymous, Route("account")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AccountService _accountService;

        public AccountController(ILogger<AccountController> logger, AccountService accountService)
        {
            _accountService = accountService;
            _logger = logger;
        }
        [HttpPost, Route("externallogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            return _accountService.ExernalLogin(provider, redirectUrl);
        }

        [Route("googleresponse")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _accountService.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await _accountService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                var callBackResult = await _accountService.ExternalLoginCallback();
                // Get the email claim value
                if (callBackResult)
                {
                    return LocalRedirect(returnUrl);
                }
                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";
                return View("Error");
            }
        }
    }
}
