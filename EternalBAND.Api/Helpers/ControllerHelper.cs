using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EternalBAND.Api.Helpers
{
    public class ControllerHelper
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _context;
        public ControllerHelper(UserManager<Users> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Users> GetCurrentUserAsync(ClaimsPrincipal user )
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task<bool> IsUserExist(string id)
        { 
            var user = await _context.Users.FindAsync(id);
            return user != null;
        }
    }
}
