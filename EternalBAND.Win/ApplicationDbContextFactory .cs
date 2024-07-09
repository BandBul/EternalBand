using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using EternalBAND.DataAccess;

namespace EternalBAND.Win
{
    // https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=vs
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("EternalBAND.Migrations"));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
