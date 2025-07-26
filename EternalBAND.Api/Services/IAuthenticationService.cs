using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ApiContract;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace EternalBAND.Api.Services
{
    public interface IAuthenticationService
    {
        Task<string> CreateTokenAsync(string userName);
        Task CreateUserAsync(SignUpInputContract signUpContract);
        bool IsTokenInCookie(IRequestCookieCollection cookies);
        ClaimsPrincipal? ValidateToken(string token, out SecurityToken validatedToken);
        Task<Users> ValidateLogin(string email, string pass);
    }
}
