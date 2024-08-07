using EternalBAND.Api.Options;
using EternalBAND.Common;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ApiContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace EternalBAND.Api.Services
{
    public class AuthenticationService
    {
        private readonly JwtTokenOptions jwtTokenSettings;
        private readonly UserManager<Users> userManager;
        private readonly IUserStore<Users> userStore;
        private readonly IUserEmailStore<Users> emailStore;
        private readonly ILogger<AuthenticationService> logger;
        private readonly SignInManager<Users> signInManager;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public AuthenticationService(
            IOptions<JwtTokenOptions> jwtTokenSettings, 
            UserManager<Users> userManager, 
            IUserStore<Users> userStore, 
            ILogger<AuthenticationService> logger, 
            SignInManager<Users> signInManager,
            JwtSecurityTokenHandler tokenHandler)
        {
            this.jwtTokenSettings = jwtTokenSettings.Value;
            this.userManager = userManager;
            this.userStore = userStore;
            this.emailStore = GetEmailStore();
            this.logger = logger;
            this.signInManager = signInManager;
            this.tokenHandler = tokenHandler;

        }

        // TODO we do not to mark this method async because token generation is almost static job,
        // we may think to refactor async call as sync here
        public async Task<string> CreateTokenAsync(string userName)
        {
            var user = await userManager.FindByEmailAsync(userName);
            var roles = await userManager.GetRolesAsync(user);
            return CreateToken(user, roles);
        }
        public async Task CreateUserAsync(SignUpInputContract signUpContract)
        {
            var user = InstentiateUser();
            user.PhotoPath = "/images/user_photo/profile.png";
            user.RegistrationDate = DateTime.Now;
            await userStore.SetUserNameAsync(user, signUpContract.Username, CancellationToken.None);
            await emailStore.SetEmailAsync(user, signUpContract.Username, CancellationToken.None);
            var result = await userManager.CreateAsync(user, signUpContract.Password);
            await userManager.AddToRoleAsync(user, Constants.NormalUserRoleName);
            if (result.Succeeded)
            {
                logger.LogInformation("User created a new account with password.");
                //TODO: need confirmation email integration
                await signInManager.SignInAsync(user, isPersistent: false);
            }
            else
            {
                throw new Exception($"Problem happen during user creation: {string.Join(Environment.NewLine, result.Errors.Select(s => s.Description))}");

            }
        }

        private IUserEmailStore<Users> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Users>)userStore;
        }

        private Users InstentiateUser()
        {
            try
            {
                return Activator.CreateInstance<Users>();
            }

            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Users)}'. " +
                    $"Ensure that '{nameof(Users)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private string CreateToken(Users? user, IEnumerable<string> roles)
        {
            var key = Encoding.ASCII.GetBytes(jwtTokenSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user?.Id.ToString()),
                new Claim(ClaimTypes.Name, user?.UserName),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = jwtTokenSettings.Issuer,
                Audience = jwtTokenSettings.Audience,
                // TODO add  this to appsettings
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
