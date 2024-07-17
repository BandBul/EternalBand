using EternalBAND.Api.Options;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ApiContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EternalBAND.Win.Controllers.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationWebController : ControllerBase
    {
        private readonly SignInManager<Users> signInManager;
        private readonly JwtTokenOptions jwtTokenSettings;
        private readonly ILogger<AuthenticationWebController> logger;
        private readonly UserManager<Users> userManager;

        // TODO create an AuthenticationService to separate logic
        public AuthenticationWebController(
            ILogger<AuthenticationWebController> logger, 
            SignInManager<Users> signInManager, 
            IOptions<JwtTokenOptions> jwtTokenSettings,
            UserManager<Users> userManager
            )
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.jwtTokenSettings = jwtTokenSettings.Value;
            this.userManager = userManager;
        }
        // for now UserName and email is same later we should separate them
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserContracts loginUser)
        {
            var result = await signInManager.PasswordSignInAsync(loginUser.Username, loginUser.Password, loginUser.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                {
                    return Unauthorized("User is not allowed");
                }

                else if (result.IsLockedOut)
                {
                    return Unauthorized("User account is locked");
                }

                else if (result.RequiresTwoFactor)
                {
                    return Unauthorized("Need 2 FActor Authentication");
                }

                else
                {
                    return Unauthorized("Username or password is incorrect");
                }
            }
            var user = await userManager.FindByEmailAsync(loginUser.Username);
            var roles = await userManager.GetRolesAsync(user);
            string tokenString = CreateToken(user, roles);

            return Ok(new { Token = tokenString });
        }

        private string CreateToken(Users? user, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtTokenSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
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
