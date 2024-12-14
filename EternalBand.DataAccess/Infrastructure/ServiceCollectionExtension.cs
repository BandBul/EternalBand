using EternalBAND.DataAccess.Repository;
using EternalBAND.DomainObjects;
using Microsoft.Extensions.DependencyInjection;

namespace EternalBAND.DataAccess.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataAccessInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<BaseRepository,BaseRepository<UserProfileControl>>();
            services.AddScoped<BaseRepository,BaseRepository<Notification>>();
            services.AddScoped<BaseRepository,BaseRepository<Users>>();
            services.AddScoped<BaseRepository,BaseRepository<Instruments>>();
            services.AddScoped<BaseRepository,BaseRepository<Posts>>();
            services.AddScoped<BaseRepository,BaseRepository<PostTypes>>();
            services.AddScoped<BaseRepository,BaseRepository<Messages>>();
            services.AddScoped<BaseRepository,BaseRepository<Contacts>>();
            services.AddScoped<BaseRepository,BaseRepository<Blogs>>();
            services.AddScoped<RepositoryProvider>();

            return services;
        }
    }
}
