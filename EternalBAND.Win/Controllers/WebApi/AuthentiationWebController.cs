using EternalBAND.Api.Services;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ApiContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EternalBAND.Api.Extensions;
using Microsoft.AspNetCore.Authentication;
using AuthenticationService = EternalBAND.Api.Services.AuthenticationService;

namespace EternalBAND.Win.Controllers.WebApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationWebController : ControllerBase
    {
        private readonly SignInManager<Users> signInManager;
        private readonly AuthenticationService authenticationService;
        private readonly ILogger<AuthenticationWebController> logger;

        // TODO create an AuthenticationService to separate logic
        public AuthenticationWebController(
            ILogger<AuthenticationWebController> logger, 
            SignInManager<Users> signInManager,
            AuthenticationService authenticationService
            )
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.authenticationService = authenticationService;
        }
        // TODO for now UserName and email is same later we should separate them
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputContract loginUser)
        {
            var result = await signInManager.PasswordSignInAsync(loginUser.Username, loginUser.Password, loginUser.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return this.HandleSignInFailure(result);
            }
            string tokenString = await authenticationService.CreateTokenAsync(loginUser.Username);

            return Ok(new LoginOutputContract { Token = tokenString });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out of the standard user-password Identity
            await signInManager.SignOutAsync();
            // Sign out of the External authentication scheme
            await HttpContext.SignOutAsync();

            return Ok();
        }
    }
}
