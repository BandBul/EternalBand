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

namespace EternalBAND.Api.Services
{
    public class AuthenticationService
    {
        private readonly JwtTokenOptions jwtTokenSettings;
        private readonly UserManager<Users> userManager;

        public AuthenticationService(IOptions<JwtTokenOptions> jwtTokenSettings, UserManager<Users> userManager)
        {
            this.jwtTokenSettings = jwtTokenSettings.Value;
            this.userManager = userManager;
        }

        // TODO we do not to mark this method async because token generation is almost static job,
        // we may think to refactor async call as sync here
        public async Task<string> CreateTokenAsync(string userName)
        {
            var user = await userManager.FindByEmailAsync(userName);
            var roles = await userManager.GetRolesAsync(user);
            return CreateToken(user, roles);
        }

        private string CreateToken(Users? user, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
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
