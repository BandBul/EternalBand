using EternalBAND.Common;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EternalBAND.DataAccess;

public class ApplicationDbContext : IdentityDbContext<Users>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserProfileControl> UserProfileControl { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Instruments> Instruments { get; set; }
    public DbSet<Posts> Posts { get; set; }
    public DbSet<PostTypes> PostTypes { get; set; }
    public DbSet<Messages> Messages { get; set; }
    public DbSet<Contacts> Contacts { get; set; }
    public DbSet<Logs> Logs { get; set; }
    public DbSet<ErrorLogs> ErrorLogs { get; set; }
    public DbSet<Blogs> Blogs { get; set; }
    public DbSet<MessageBox> MessageBoxes { get; set; }

    public async Task Commit()
    {
        await SaveChangesAsync();
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<PostTypes>().HasData(
            new PostTypes
            {
                Id = 1,
                FilterText = "Grup Arıyorum",
                Type = PostTypeName.Group.ToString(),
                TypeText = "Grup İlanları",
                Active = true,
                // 3 March 2024 00.16.00
                AddedDate = new DateTime(2024,3,3,0,16,0),
                PostCreateText = "Grup Arıyorum"

            },
            new PostTypes
            {
                Id = 2,
                FilterText = "Müzisyen Arıyorum",
                Type = PostTypeName.Musician.ToString(),
                TypeText = "Müzisyen İlanları",
                Active = true,
                // 3 March 2024 00.16.00
                AddedDate = new DateTime(2024, 3, 3, 0, 16, 0),
                PostCreateText = "Müzisyen Arıyorum",
               
            },
            new PostTypes
            {
                Id = 3,
                FilterText = "Ders Almak İstiyorum",
                TypeText = "Ders İlanları",
                Type = PostTypeName.Lesson.ToString(),
                Active = true,
                // 3 March 2024 00.16.00
                AddedDate = new DateTime(2024, 3, 3, 0, 16, 0),
                PostCreateText = "Ders Vermek İstiyorum",
                
            }
        );

        builder.Entity<Instruments>().HasData(
            new Instruments
            {
                Id = 1,
                Instrument = "Gitar",
                InstrumentShort = "Guitar",
                IsActive = true,
            },
            new Instruments
            {
                Id = 2,
                Instrument = "Bas Gitar",
                InstrumentShort = "Bass Guitar",
                IsActive = true,
            },
            new Instruments
            {
                Id = 3,
                Instrument = "Davul",
                InstrumentShort = "Drum",
                IsActive = true,
            },
            new Instruments
            {
                Id = 4,
                Instrument = "Piano",
                InstrumentShort = "Piano",
                IsActive = true,
            },
            new Instruments
            {
                Id = 5,
                Instrument = "Klavye",
                InstrumentShort = "Keyboard",
                IsActive = true,
            },
            new Instruments
            {
                Id = 6,
                Instrument = "Saksafon",
                InstrumentShort = "Saxophone",
                IsActive = true,
            },
            new Instruments
            {
                Id = 7,
                Instrument = "Keman",
                InstrumentShort = "Violin",
                IsActive = true,
            },
            new Instruments
            {
                Id = 8,
                Instrument = "Vokal",
                InstrumentShort = "Vocal",
                IsActive = true,
            },
            new Instruments
            {
                Id = 9,
                Instrument = "Kontrabas",
                InstrumentShort = "Kontrabas",
                IsActive = true,
            }
        );

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = Constants.AdminRoleId,
                Name = Constants.AdminRoleName,
                NormalizedName = Constants.AdminRoleName.ToUpperInvariant(),
            },
            new IdentityRole
            {
                Id = Constants.NormalUserId,
                Name = Constants.NormalUserRoleName,
                NormalizedName = Constants.NormalUserRoleName.ToUpperInvariant(),
            }
        );
        
        
        builder.Entity<Users>().HasData(new Users // Pass Af9CCdzXYYxLQSXR
        {
            Id = Constants.AdminUserId,
            UserName = "admin@bandbul.com",
            NormalizedUserName = "ADMIN@BANDBUL.COM",
            Email = "admin@bandbul.com",
            NormalizedEmail = "ADMIN@BANDBUL.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAEAACcQAAAAEKI5PN2jzhcrAOnOBDzALJvU65YuCMAKjwwQwaCnxR0UpCdXn8J4tueymis2QexFAA==",
            SecurityStamp = "E2B3O7QZENNRGFV2LTBADCYOV7PA4BYQ",
            ConcurrencyStamp = "936d1570-9809-4120-9359-b16af62456fd",
            Name = "Super",
            Surname = "Admin",
            LockoutEnabled = true,
            TwoFactorEnabled = false,
            PhoneNumberConfirmed = false,
            AccessFailedCount = 0,
            PhoneNumber = "5000000000",
            PhotoPath = "/images/user_photo/profile.png",
            FullName = "Super Admin",
        });
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = Constants.AdminRoleId,
            UserId = Constants.AdminUserId
        });
        
        base.OnModelCreating(builder);
    }
}