using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EternalBAND.Api.Services;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using EternalBAND.Api;

namespace EternalBAND.Controllers
{
    [AllowAnonymous, Route("account")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
        private readonly AccountService accountService;

        public AccountController(
            ILogger<AccountController> logger, 
            AccountService accountService)
        {
            this.accountService = accountService;
            this.logger = logger;
        }
        [HttpPost, Route("externallogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            logger.LogInformation($"{provider} signin called with redirecturl : {redirectUrl}");
            return accountService.ExernalLogin(provider, redirectUrl);
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
            var info = await accountService.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await accountService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);
            if (result.Succeeded)
            {
                logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            // No confirm needed for external login
            //if (result.IsNotAllowed)
            //{
            //    ModelState.AddModelError(string.Empty, "Lütfen mail adresinizi onaylayınız.");
            //    return RedirectToPage("./Login");
            //}
            // failure means first login
            else
            {
                var callBackResult = await accountService.ExternalLoginCallback(info);
                if (callBackResult)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ViewBag.ErrorTitle = $"E-posta talebi alınamadı: {info.LoginProvider}";
                    ViewBag.ErrorMessage = "Lütfen destek için info@bandbul.com adresiyle iletişime geçin.";
                    return View("Error");
                }
            }
        }
    }
}
