using Azure;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using EternalBAND.DomainObjects.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EternalBAND.Win.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorWebController : ControllerBase
    {
        public ErrorWebController()
        {
        }

        [HttpGet, Route("webError/{code}")]
        public IActionResult Error(int code)
        {
            switch (code)
            {
                case (int)HttpStatusCode.NotFound :
                    return NotFound("page is not found");
                case (int)HttpStatusCode.Unauthorized:
                    return Unauthorized("USer is not logged in for this action");
                case (int)HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden, "User is not authorized for this action");
                default:
                    return Problem($"Error happened with '{code}' status code");
            }
        }
    }
}
