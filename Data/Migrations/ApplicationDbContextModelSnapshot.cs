﻿// <auto-generated />
using System;
using EternalBAND.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EternalBAND.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EternalBAND.Models.Blogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("HtmlText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SeoLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("EternalBAND.Models.Contacts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsDone")
                        .HasColumnType("bit");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("EternalBAND.Models.ErrorLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("LongMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PageOrMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogs");
                });

            modelBuilder.Entity("EternalBAND.Models.Instruments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Instrument")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstrumentShort")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Instruments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Instrument = "Gitar",
                            InstrumentShort = "Guitar",
                            IsActive = true
                        },
                        new
                        {
                            Id = 2,
                            Instrument = "Bas Gitar",
                            InstrumentShort = "Bass Guitar",
                            IsActive = true
                        },
                        new
                        {
                            Id = 3,
                            Instrument = "Davul",
                            InstrumentShort = "Drum",
                            IsActive = true
                        },
                        new
                        {
                            Id = 4,
                            Instrument = "Piano",
                            InstrumentShort = "Piano",
                            IsActive = true
                        },
                        new
                        {
                            Id = 5,
                            Instrument = "Klavye",
                            InstrumentShort = "Keyboard",
                            IsActive = true
                        },
                        new
                        {
                            Id = 6,
                            Instrument = "Saksafon",
                            InstrumentShort = "Saxophone",
                            IsActive = true
                        },
                        new
                        {
                            Id = 7,
                            Instrument = "Keman",
                            InstrumentShort = "Violin",
                            IsActive = true
                        },
                        new
                        {
                            Id = 8,
                            Instrument = "Vokal",
                            InstrumentShort = "Vocal",
                            IsActive = true
                        },
                        new
                        {
                            Id = 9,
                            Instrument = "Kontrabas",
                            InstrumentShort = "Kontrabas",
                            IsActive = true
                        });
                });

            modelBuilder.Entity("EternalBAND.Models.Logs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("PageOrMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("EternalBAND.Models.Messages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MessageGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReceiverUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SenderUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverUserId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("EternalBAND.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiveUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RedirectLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedElementId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiveUserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("EternalBAND.Models.PostTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeShort")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PostTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            AddedDate = new DateTime(2023, 9, 14, 23, 33, 16, 431, DateTimeKind.Local).AddTicks(4389),
                            Type = "Müzisyen Arıyorum",
                            TypeShort = "Musician"
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            AddedDate = new DateTime(2023, 9, 14, 23, 33, 16, 431, DateTimeKind.Local).AddTicks(4440),
                            Type = "Grup Arıyorum",
                            TypeShort = "Group"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            AddedDate = new DateTime(2023, 9, 14, 23, 33, 16, 431, DateTimeKind.Local).AddTicks(4444),
                            Type = "Ders Vermek İstiyorum",
                            TypeShort = "Lesson"
                        });
                });

            modelBuilder.Entity("EternalBAND.Models.Posts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AddedByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("AdminConfirmation")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("AdminConfirmationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("AdminConfirmationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<Guid?>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HTMLText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InstrumentsId")
                        .HasColumnType("int");

                    b.Property<string>("Photo1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostTypesId")
                        .HasColumnType("int");

                    b.Property<string>("SeoLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddedByUserId");

                    b.HasIndex("AdminConfirmationUserId");

                    b.HasIndex("InstrumentsId");

                    b.HasIndex("PostTypesId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("EternalBAND.Models.SystemInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SystemInfo");

                    b.HasData(
                        new
                        {
                            Id = 12,
                            Desc = "Adres (Boş bırakırsanız göstermez)",
                            Type = "site-address",
                            Value = "Istanbul"
                        },
                        new
                        {
                            Id = 11,
                            Desc = "Site Telefon Numarası (Boş bırakırsanız göstermez)",
                            Type = "site-phone",
                            Value = "0850"
                        },
                        new
                        {
                            Id = 10,
                            Desc = "Site Facebook Adres (Boş bırakırsanız göstermez)",
                            Type = "site-twitter",
                            Value = "https://twitter.com"
                        },
                        new
                        {
                            Id = 9,
                            Desc = "Site Instagram Adres (Boş bırakırsanız göstermez)",
                            Type = "site-instagram",
                            Value = "https://instagram.com"
                        },
                        new
                        {
                            Id = 7,
                            Desc = "Site Facebook Adres (Boş bırakırsanız göstermez)",
                            Type = "site-facebook",
                            Value = "https://facebook.com"
                        },
                        new
                        {
                            Id = 8,
                            Desc = "Site İletişim Mail Adresi (Boş bırakırsanız göstermez)",
                            Type = "site-mail-address",
                            Value = "test@mail.com"
                        },
                        new
                        {
                            Id = 1,
                            Desc = "Mail Gönderen Adres",
                            Type = "mail-sender-address",
                            Value = "testuservortex@outlook.com"
                        },
                        new
                        {
                            Id = 2,
                            Desc = "Mail Gönderen Sunucu Portu",
                            Type = "mail-sender-port",
                            Value = "587"
                        },
                        new
                        {
                            Id = 3,
                            Desc = "Mail Gönderen Sunucu Hostu",
                            Type = "mail-sender-host",
                            Value = "smtp-mail.outlook.com"
                        },
                        new
                        {
                            Id = 4,
                            Desc = "Mail Gönderen Sunucu SSL İstiyor mu?",
                            Type = "mail-sender-enable-ssl",
                            Value = "true"
                        },
                        new
                        {
                            Id = 5,
                            Desc = "Mail Gönderen Adres Şifresi",
                            Type = "mail-sender-address-password",
                            Value = "K48F7HWiFk7Abgjn"
                        },
                        new
                        {
                            Id = 13,
                            Desc = "Site Logo",
                            Type = "site-logo",
                            Value = "/images/logo-light.png"
                        },
                        new
                        {
                            Id = 14,
                            Desc = "Site Favicon",
                            Type = "site-favicon",
                            Value = "/images/favicon.ico"
                        },
                        new
                        {
                            Id = 15,
                            Desc = "Site Domain",
                            Type = "site-domain",
                            Value = "https://bandbul.checktheproject.com"
                        },
                        new
                        {
                            Id = 17,
                            Desc = "Site Sol Alt Metin",
                            Type = "site-footer-left-text",
                            Value = " It is a long established fact that a reader will be of a page reader will be of at its layout. "
                        },
                        new
                        {
                            Id = 18,
                            Desc = "Site Desc Seo için",
                            Type = "site-desc",
                            Value = "Buraya description gelecek :)"
                        },
                        new
                        {
                            Id = 19,
                            Desc = "Site Keywords Seo için",
                            Type = "site-keywords",
                            Value = "keywords , keywords 1, keywords 2"
                        },
                        new
                        {
                            Id = 16,
                            Desc = "Site Bottom Footer Text",
                            Type = "site-bottom-footer-text",
                            Value = "- Tüm hakları saklıdır."
                        },
                        new
                        {
                            Id = 6,
                            Desc = "Site Başlığı",
                            Type = "site-title",
                            Value = "EternalBAND"
                        });
                });

            modelBuilder.Entity("EternalBAND.Models.UserProfileControl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UsersId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UsersId");

                    b.ToTable("UserProfileControl");
                });

            modelBuilder.Entity("EternalBAND.Models.Users", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "248feb45-0294-450c-afd2-9c6de89023b9",
                            AccessFailedCount = 0,
                            Age = 0,
                            City = 0,
                            ConcurrencyStamp = "936d1570-9809-4120-9359-b16af62456fd",
                            Email = "admin@bandbul.com",
                            EmailConfirmed = true,
                            FullName = "Super Admin",
                            LockoutEnabled = true,
                            Name = "Super",
                            NormalizedEmail = "ADMIN@BANDBUL.COM",
                            NormalizedUserName = "ADMIN@BANDBUL.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEKI5PN2jzhcrAOnOBDzALJvU65YuCMAKjwwQwaCnxR0UpCdXn8J4tueymis2QexFAA==",
                            PhoneNumber = "5000000000",
                            PhoneNumberConfirmed = false,
                            PhotoPath = "/img/user_photo/profile.png",
                            RegistrationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SecurityStamp = "E2B3O7QZENNRGFV2LTBADCYOV7PA4BYQ",
                            Surname = "Admin",
                            TwoFactorEnabled = false,
                            UserName = "admin@bandbul.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "39d321fc-0911-4412-a19e-98fb7d068440",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "a3641119-ff91-4eca-aa32-120c36d61d1a",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "248feb45-0294-450c-afd2-9c6de89023b9",
                            RoleId = "39d321fc-0911-4412-a19e-98fb7d068440"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("EternalBAND.Models.Messages", b =>
                {
                    b.HasOne("EternalBAND.Models.Users", "ReceiverUser")
                        .WithMany()
                        .HasForeignKey("ReceiverUserId");

                    b.HasOne("EternalBAND.Models.Users", "SenderUser")
                        .WithMany()
                        .HasForeignKey("SenderUserId");

                    b.Navigation("ReceiverUser");

                    b.Navigation("SenderUser");
                });

            modelBuilder.Entity("EternalBAND.Models.Notification", b =>
                {
                    b.HasOne("EternalBAND.Models.Users", "ReceiveUser")
                        .WithMany()
                        .HasForeignKey("ReceiveUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReceiveUser");
                });

            modelBuilder.Entity("EternalBAND.Models.Posts", b =>
                {
                    b.HasOne("EternalBAND.Models.Users", "AddedByUser")
                        .WithMany()
                        .HasForeignKey("AddedByUserId");

                    b.HasOne("EternalBAND.Models.Users", "AdminConfirmationUser")
                        .WithMany()
                        .HasForeignKey("AdminConfirmationUserId");

                    b.HasOne("EternalBAND.Models.Instruments", "Instruments")
                        .WithMany()
                        .HasForeignKey("InstrumentsId");

                    b.HasOne("EternalBAND.Models.PostTypes", "PostTypes")
                        .WithMany()
                        .HasForeignKey("PostTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddedByUser");

                    b.Navigation("AdminConfirmationUser");

                    b.Navigation("Instruments");

                    b.Navigation("PostTypes");
                });

            modelBuilder.Entity("EternalBAND.Models.UserProfileControl", b =>
                {
                    b.HasOne("EternalBAND.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EternalBAND.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EternalBAND.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EternalBAND.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EternalBAND.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
