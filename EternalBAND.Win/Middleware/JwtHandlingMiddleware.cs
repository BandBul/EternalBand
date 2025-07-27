using EternalBAND.Api.Services;
using EternalBAND.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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

        // TODO implement two jwt token check logic one for swagger/api it will check Authorization header
        // second is for MVC/Razor requests which will be controlled by cookie ( delete/add/update cookie)
        public async Task InvokeAsync(HttpContext context)
        {

            var excludedPaths = new[] { UrlConstants.Login, UrlConstants.Logout, UrlConstants.React, UrlConstants.StaticPath };
            var requestPath = context.Request.Path;

            if (excludedPaths.Any(p => requestPath != null && (requestPath.Value.StartsWith(p) || requestPath.Value.EndsWith(p))))
            {
                await _next(context);
                return;
            }

            var isApiRequest = requestPath.StartsWithSegments("/api") ||
                            context.Request.Headers["Accept"].ToString().Contains("application/json");
            var token = context.Request.Cookies[Constants.AccessTokenCookieName];

            // WebAPI
            if (isApiRequest)
            {
                if (!context.Request.Headers.ContainsKey("Authorization") && !string.IsNullOrEmpty(token))
                {
                    context.Request.Headers["Authorization"] = $"Bearer {token}";
                }

                else if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    var tokenString = authHeader.ToString();
                    token = 
                        tokenString.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ?
                        tokenString.Substring("Bearer ".Length).Trim() :
                        tokenString;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized - Token missing.");
                    return;
                }
            }

            // MVC
            else
            {
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.Redirect($"/{UrlConstants.Login}");
                    return;
                }
            }

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
            }
            catch (Exception ex)
            {
                context.Response.Cookies.Delete(Constants.AccessTokenCookieName);
                _logger.LogError(ex, "Error parsing the JWT token.");
                if (isApiRequest)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token");
                    return;
                }

                context.Response.Redirect($"/{UrlConstants.Login}");
                return;
            }

            // Continue processing if token is valid or does not exist
            await _next(context);
        }

    }
}
