using EternalBAND.Api.Helpers;
using EternalBAND.Api.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EternalBAND.Api.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApiInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, MailSender>();
            services.AddScoped<HomeService>();
            services.AddScoped<UserService>();
            services.AddScoped<AdminService>();
            services.AddScoped<AccountService>();
            services.AddScoped<MessageService>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<HubController>();
            services.AddScoped<ControllerHelper>();
            services.AddScoped<BroadCastingManager>();

            return services;
        }
    }
}
