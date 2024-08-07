using EternalBAND.Api.Services;
using EternalBAND.Controllers;
using Microsoft.AspNetCore.Mvc;
using EternalBAND.Api.Extensions;
using System.Security.Claims;
using EternalBAND.DomainObjects.ApiContract;

namespace EternalBAND.Win.Controllers.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountWebController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly AccountService accountService;
        private readonly AuthenticationService authenticationService;

        public AccountWebController(ILogger<AccountController> logger, AccountService accountService, AuthenticationService authenticationService)
        {
            this.accountService = accountService;
            this.logger = logger;
            this.authenticationService = authenticationService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider">third party provicer name like Google</param>
        /// <param name="returnUrl">Where should be returned after successfully login</param>
        /// <returns></returns>
        // TODO use returnUrl
        [HttpPost("externallogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            return accountService.ExernalLoginWithReturnUrl(provider, returnUrl);
        }

        [HttpGet("googleresponse")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToPage("./Login");
            }
            var info = await accountService.GetExternalLoginInfoAsync();
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var returnUrl = info?.AuthenticationProperties?.Items["returnUrl"] ?? Url.Content("~/");

            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await accountService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);
            if (!result.Succeeded)
            {
                return this.HandleSignInFailure(result);
            }

            var callBackResult = await accountService.ExternalLoginCallback(info);
            // Get the email claim value
            if (callBackResult)
            {
                //TODO: Email and user name should be different here
                var token = await authenticationService.CreateTokenAsync(email);
                return Ok(new LoginOutputContract { Token = token, ReturnUrl = returnUrl });
            }
            // TODO what should we exactly return here ?
            return Problem();
        }
    }
}
