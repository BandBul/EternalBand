using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace EternalBAND.Api.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApiInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBaseEmailSender, EmailSender>();
            services.AddScoped<HomeService>();
            services.AddScoped<UserService>();
            services.AddScoped<AdminService>();
            services.AddScoped<AccountService>();
            services.AddScoped<MessageService>();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<HubController>();
            services.AddScoped<ControllerHelper>();
            services.AddScoped<BroadCastingManager>();
            return services;
        }
    }
}
