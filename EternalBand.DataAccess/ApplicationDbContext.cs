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
    public DbSet<SystemInfo> SystemInfo { get; set; }
    public DbSet<Logs> Logs { get; set; }
    public DbSet<ErrorLogs> ErrorLogs { get; set; }
    public DbSet<Blogs> Blogs { get; set; }

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
                Type = "Müzisyen Arıyorum",
                TypeShort = "Musician",
                Active = true,
                AddedDate = DateTime.Now
            },
            new PostTypes
            {
                Id = 2,
                Type = "Grup Arıyorum",
                TypeShort = "Group",
                Active = true,
                AddedDate = DateTime.Now
            },
            new PostTypes
            {
                Id = 3,
                Type = "Ders Vermek İstiyorum",
                TypeShort = "Lesson",
                Active = true,
                AddedDate = DateTime.Now
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
            PhotoPath = "/img/user_photo/profile.png",
            FullName = "Super Admin",
        });
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = Constants.AdminRoleId,
            UserId = Constants.AdminUserId
        });
        builder.Entity<SystemInfo>().HasData(
            new SystemInfo
            {
                Id = 12,
                Type = "site-address",
                Value = "Istanbul",
                Desc = "Adres (Boş bırakırsanız göstermez)"
            },
            new SystemInfo
            {
                Id = 11,
                Type = "site-phone",
                Value = "0850",
                Desc = "Site Telefon Numarası (Boş bırakırsanız göstermez)"
            },
            new SystemInfo
            {
                Id = 10,
                Type = "site-twitter",
                Value = "https://twitter.com",
                Desc = "Site Facebook Adres (Boş bırakırsanız göstermez)"
            },
            new SystemInfo
            {
                Id = 9,
                Type = "site-instagram",
                Value = "https://instagram.com",
                Desc = "Site Instagram Adres (Boş bırakırsanız göstermez)"
            },
            new SystemInfo
            {
                Id = 7,
                Type = "site-facebook",
                Value = "https://facebook.com",
                Desc = "Site Facebook Adres (Boş bırakırsanız göstermez)"
            },
            new SystemInfo
            {
                Id = 8,
                Type = "site-mail-address",
                Value = "test@mail.com",
                Desc = "Site İletişim Mail Adresi (Boş bırakırsanız göstermez)"
            },
            new SystemInfo
            {
                Id = 1,
                Type = "mail-sender-address",
                Value = "testuservortex@outlook.com",
                Desc = "Mail Gönderen Adres"
            },
            new SystemInfo
            {
                Id = 2,
                Type = "mail-sender-port",
                Value = "587",
                Desc = "Mail Gönderen Sunucu Portu"
            },
            new SystemInfo
            {
                Id = 3,
                Type = "mail-sender-host",
                Value = "smtp-mail.outlook.com",
                Desc = "Mail Gönderen Sunucu Hostu"
            },
            new SystemInfo
            {
                Id = 4,
                Type = "mail-sender-enable-ssl",
                Value = "true",
                Desc = "Mail Gönderen Sunucu SSL İstiyor mu?"
            },
            new SystemInfo
            {
                Id = 5,
                Type = "mail-sender-address-password",
                Value = "K48F7HWiFk7Abgjn",
                Desc = "Mail Gönderen Adres Şifresi"
            },
            new SystemInfo
            {
                Id = 13,
                Type = "site-logo",
                Value = "/images/logo-light.png",
                Desc = "Site Logo"
            },
            new SystemInfo
            {
                Id = 14,
                Type = "site-favicon",
                Value = "/images/favicon.ico",
                Desc = "Site Favicon"
            },   new SystemInfo
            {
                Id = 15,
                Type = "site-domain",
                Value = "https://bandbul.checktheproject.com",
                Desc = "Site Domain"
            }
            ,   new SystemInfo
            {
                Id = 17,
                Type = "site-footer-left-text",
                Value = " It is a long established fact that a reader will be of a page reader will be of at its layout. ",
                Desc = "Site Sol Alt Metin"
            } ,   new SystemInfo
            {
                Id = 18,
                Type = "site-desc",
                Value = "Buraya description gelecek :)",
                Desc = "Site Desc Seo için"
            },   new SystemInfo
            {
                Id = 19,
                Type = "site-keywords",
                Value = "keywords , keywords 1, keywords 2",
                Desc = "Site Keywords Seo için"
            }
            ,   new SystemInfo
            {
                Id = 16,
                Type = "site-bottom-footer-text",
                Value = "- Tüm hakları saklıdır.",
                Desc = "Site Bottom Footer Text"
            },
            new SystemInfo
            {
                Id = 6,
                Type = "site-title",
                Value = "EternalBAND",
                Desc = "Site Başlığı"
            });
        base.OnModelCreating(builder);
    }
}