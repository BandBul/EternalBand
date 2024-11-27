using EternalBAND.Api.Options;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EternalBAND.Win.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        private const string Bearer = "Bearer";
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

                c.AddSecurityDefinition(Bearer, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = Bearer,
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
                                Id = Bearer
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
                options.DefaultAuthenticateScheme = Bearer;
                options.DefaultChallengeScheme = Bearer;
            })
            .AddJwtBearer(options =>
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
            });

           

            return services;
        }
    }
}
