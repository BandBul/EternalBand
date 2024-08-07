using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Api.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult HandleSignInFailure(this ControllerBase controller, Microsoft.AspNetCore.Identity.SignInResult result)
        {
            if (result.IsNotAllowed)
            {
                return controller.StatusCode(StatusCodes.Status403Forbidden);
            }

            else if (result.IsLockedOut)
            {
                return controller.StatusCode(StatusCodes.Status403Forbidden);
            }
            // TODO add meaningful message here
            else if (result.RequiresTwoFactor)
            {
                return controller.StatusCode(StatusCodes.Status401Unauthorized);
            }

            else
            {
                return controller.StatusCode(StatusCodes.Status401Unauthorized);
            }
        }
    }
}
