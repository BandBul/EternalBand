using EternalBAND.Common;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;

namespace EternalBAND.DataAccess
{
    public static class DefaultUserAdditionHelper
    {
        public static Users CreateAdminUser(string userName, string password)
        {
            var user = new Users
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Email = userName,
                NormalizedEmail = userName.ToUpperInvariant(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                Name = "Admin",
                Surname = "",
                LockoutEnabled = true,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                PhoneNumber = "5000000000",
                PhotoPath = Constants.DefaultPhotoPath,
                FullName = "Admin"
            };

            var hasher = new PasswordHasher<Users>();
            var adminPasswordHash = hasher.HashPassword(user, password);
            user.PasswordHash = adminPasswordHash;

            return user;
        }
    }
}
