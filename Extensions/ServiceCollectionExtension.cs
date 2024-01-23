using EternalBAND.Api;
using EternalBAND.Api.Services;
using EternalBAND.DataAccess.Repository;
using EternalBAND.DomainObjects;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace EternalBAND.Win.Extensions
{
    public static class ServiceCollectionExtension
    {
        //TODO change the project and folder name/structure this is actually IOC registration but name seems its just extension
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<BaseRepository,BaseRepository<UserProfileControl>>();
            services.AddScoped<BaseRepository,BaseRepository<Notification>>();
            services.AddScoped<BaseRepository,BaseRepository<Users>>();
            services.AddScoped<BaseRepository,BaseRepository<Instruments>>();
            services.AddScoped<BaseRepository,BaseRepository<Posts>>();
            services.AddScoped<BaseRepository,BaseRepository<PostTypes>>();
            services.AddScoped<BaseRepository,BaseRepository<Messages>>();
            services.AddScoped<BaseRepository,BaseRepository<Contacts>>();
            services.AddScoped<BaseRepository,BaseRepository<SystemInfo>>();
            services.AddScoped<BaseRepository,BaseRepository<Logs>>();
            services.AddScoped<BaseRepository,BaseRepository<ErrorLogs>>();
            services.AddScoped<BaseRepository,BaseRepository<Blogs>>();

            services.AddScoped<RepositoryProvider>();

            return services;
        }

        public static IServiceCollection RegisterBusinessDomainObject(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, MailSender>();
            services.AddScoped<HomeService>();
            services.AddScoped<MessageService>();
            services.AddScoped<HubController>();
            return services;
        }

            
    }
}
