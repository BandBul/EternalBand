using EternalBAND.Api.Options;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EternalBAND.Api.Services
{
    public class AccountService
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly GoogleOptions googleSettings;
        public AccountService(SignInManager<Users> signInManager, UserManager<Users> userManager, IOptions<GoogleOptions> googleOptions)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.googleSettings = googleOptions.Value;
        }
        public ChallengeResult ExernalLogin(string provider, string redirectUrl) 
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public ChallengeResult ExernalLoginWithReturnUrl(string provider, string returnUrl)
        {
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, googleSettings.RedirectUrl);
            properties.Items["returnUrl"] = returnUrl;
            return new ChallengeResult(provider, properties);
        }

        public async Task<bool> ExternalLoginCallback(ExternalLoginInfo info)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                // Create a new user without password if we do not have a user already
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new Users
                    {
                        UserName = email,
                        Email = email
                    };
                    await userManager.CreateAsync(user);
                }
                // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                await userManager.AddLoginAsync(user, info);
                await signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }

            return false;
        }

        public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
        {
            return await signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey)
        {
            return await signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent: false, bypassTwoFactor: true);
        }

    }
}
