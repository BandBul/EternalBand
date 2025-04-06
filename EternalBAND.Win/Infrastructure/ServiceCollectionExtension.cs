using EternalBAND.Api.Options;
using EternalBAND.Common;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EternalBAND.Win.Infrastructure
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SmtpOptionKey));
            services.Configure<SiteGeneralOptions>(configuration.GetSection(SiteGeneralOptions.SiteGeneralOptionKey));
            services.Configure<JwtTokenOptions>(configuration.GetSection(JwtTokenOptions.JwtOptionKey));
            services.Configure<NotificationOptions>(configuration.GetSection(NotificationOptions.NotificationOptionKey));
            services.Configure<GoogleOptions>(configuration.GetSection(GoogleOptions.GoogleOptionsKey));

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Add services to the container.
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("EternalBAND.Migrations")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var info = new OpenApiInfo()
            {
                Title = "Bandbul Documentation",
                Version = "v1",
                Description = "Herkes icin Muzik",
            };

            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
        {
            services.AddIdentity<Users, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User", "Admin"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, GoogleOptions googleSettings, JwtTokenOptions jwtTokenOptions)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenOptions.Secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtTokenOptions.Issuer,
                    ValidAudience = jwtTokenOptions.Audience,
                };
            })
            .AddGoogle(opt =>
            {
                opt.ClientId = googleSettings.ClientId;
                opt.ClientSecret = googleSettings.ClientSecret;
                opt.SignInScheme = IdentityConstants.ExternalScheme;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/{UrlConstants.Login}";
                options.AccessDeniedPath = $"/{UrlConstants.AccessDenied}";
                options.LogoutPath = $"/{UrlConstants.Logout}";
                options.ExpireTimeSpan = TimeSpan.FromHours(3);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            return services;
        }
    }
}
