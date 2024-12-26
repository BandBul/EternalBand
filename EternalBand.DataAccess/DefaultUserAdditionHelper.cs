using EternalBAND.Common;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;

namespace EternalBAND.DataAccess
{
    public static class DefaultUserAdditionHelper
    {
        public static Users CreateAdminUser(string userName, string passwordHashed, string guid)
        {
            var user = new Users
            {
                Id = guid,
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Email = userName,
                NormalizedEmail = userName.ToUpperInvariant(),
                EmailConfirmed = true,
                SecurityStamp = Constants.SecurityStampGuid,
                ConcurrencyStamp = Constants.ConcurrencyStampGuid,
                Name = "Admin",
                Surname = "",
                LockoutEnabled = true,
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                PhoneNumber = "5000000000",
                PhotoPath = Constants.DefaultPhotoPath,
                FullName = "Admin",
                PasswordHash = passwordHashed
            };

            //var hasher = new PasswordHasher<Users>();
            //var adminPasswordHash = hasher.HashPassword(user, passwordHashed);
            //user.PasswordHash = adminPasswordHash;

            return user;
        }
    }
}
