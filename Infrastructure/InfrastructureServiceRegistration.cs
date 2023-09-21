using Infrastructure.Options.Database;
using Infrastructure.Options.Hash;
using Infrastructure.Options.JWT;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            //Adding the Configuration
            services.ConfigureOptions<DatabaseOptionsSetup>();
            services.ConfigureOptions<HashOptionsSetup>();
            services.ConfigureOptions<JwtOptionsSetup>();

            services.AddDbContext<AppDbContext>((serviceProvider,options) =>
            {
                //Getting an instance of IOptions to use the options that are defined in appsettings.json (must be register and inherance from IOptions interface)
                DatabaseOptions databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

                options.UseSqlServer(databaseOptions.ConnectionString, sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount); // Max number of time to try to reconnect 
                });
            });

            return services;
        }
    }
}