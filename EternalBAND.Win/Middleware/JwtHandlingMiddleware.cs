using EternalBAND.Api.Services;
using EternalBAND.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace EternalBAND.Win.Middleware
{
    public class JwtHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtHandlingMiddleware> _logger;

        public JwtHandlingMiddleware(
            RequestDelegate next,
            ILogger<JwtHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies[Constants.AccessTokenCookieName];
            var excludedPaths = new[] { UrlConstants.Login, UrlConstants.Logout };
            var requestPath = context.Request.Path.Value?.ToLower();
            if (excludedPaths.Any(p => requestPath != null && (requestPath.StartsWith(p) || requestPath.EndsWith(p))))
            {
                await _next(context);
                return;
            }

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var authService = context.RequestServices.GetRequiredService<IAuthenticationService>();
                    // Validate JWT token
                    var principal = authService.ValidateToken(token, out SecurityToken jwtToken);

                    // Check if the token has expired
                    //if (jwtToken.ValidTo < DateTime.UtcNow.AddSeconds(3570))
                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        _logger.LogInformation("JWT token expired, redirecting to login.");
                        context.Response.Cookies.Delete(Constants.AccessTokenCookieName);
                        context.User = new ClaimsPrincipal(new ClaimsIdentity());
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        // Token is expired, redirect to the login page
                        context.Response.Redirect($"/{UrlConstants.Login}");
                        return; // Stop further processing
                    }

                    context.User = principal;

                    //if (!context.Request.Headers.ContainsKey(HttpRequestHeader.Authorization.ToString()))
                    //{
                    //    context.Request.Headers[HttpRequestHeader.Authorization.ToString()] = $"Bearer {token}";
                    //}

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error parsing the JWT token.");
                    // If an error occurs (e.g., token is invalid), redirect to login
                    context.Response.Redirect($"/{UrlConstants.Login}");
                    return;
                }
            }

            // Continue processing if token is valid or does not exist
            await _next(context);
        }

    }
}
