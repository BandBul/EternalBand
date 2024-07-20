﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EternalBAND.Win.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminWebController : ControllerBase
    {
        public AdminWebController()
        {
        }

        [HttpGet("AdminIndex")]
        public async Task<ActionResult> AdminIndex(string? userId, int postId)
        {
            return Ok("Admin done something");
        }
    }
}
