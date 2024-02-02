using Application.Common.DelegatingHandlers;
using Application.Common.Interfaces;
using FluentValidation;
using Infrastructure.Extensions;
using Infrastructure.Interceptors;
using Infrastructure.Options.Country;
using Infrastructure.Options.Database;
using Infrastructure.Options.Hash;
using Infrastructure.Options.JWT;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       IWebHostEnvironment environment)
    {
        //Adding the Configuration and validator
        services.ConfigureOptions<DatabaseOptionsSetup>()
                .AddFluentValidator<DatabaseOptions>();

        services.ConfigureOptions<HashOptionsSetup>()
                .AddFluentValidator<HashOptions>();

        services.ConfigureOptions<JwtOptionsSetup>()
                .AddFluentValidator<JwtOptions>();

        services.ConfigureOptions<CountryOptionsSetup>()
                .AddFluentValidator<CountryOptions>();

        // Adding the validators of fluent validations and need to change the service lifetime to singleton or need to inject the IServiceScopeFactory or IServiceProvider and Create a scoped service instance
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // Default lifetime (scoped)

        // Registering the interceptors 
        services.AddSingleton<AuditableEntititesInterceptor>();
        services.AddScoped<EnforcedUserRoleInterceptor>(); // need to register as a scoped if you are using the current context 

        services.AddDbContext<AppDbContext>((serviceProvider,options) =>
        {
            //Getting an instance of IOptions to use the options that are defined in appsettings.json (must be register and inherance from IOptions interface)
            DatabaseOptions databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            options.UseSqlServer(databaseOptions.ConnectionString, sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);// time to execute a command and generate an error

                sqlServerOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount); // Max number of time to try to reconnect 
            })
            // adding interceptors (can be maded in the db context configuration or here / the position of the interceptors matter )
            .AddInterceptors(serviceProvider.GetRequiredService<AuditableEntititesInterceptor>())
            .AddInterceptors(serviceProvider.GetRequiredService<EnforcedUserRoleInterceptor>());

            if (environment.IsDevelopment()) // can check if the enviroment is in development
            {
                // more detailed messages and logs (these options must only be active in dev)
                options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);

                options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            }
        });

        // Adding the HttpContext
        services.AddHttpContextAccessor();

        // Adding MemoryCaching
        services.AddMemoryCache();

        // Adding Dependencies to the DI Container
        services.AddRepositoriesAndServices();
       
        //Adding the Custom Delegating Handlers to the DI Container
        services.AddTransient<LoggingHandler>();

        // Adding the HttpClient (Transient but the httpclient is reusable it solve the pool,dns and socket problem)
        services.AddHttpClient<ICountryService, CountryService>((serviceProvider,client) => 
        {
            CountryOptions countryOptions = serviceProvider.GetRequiredService<IOptions<CountryOptions>>().Value;

            client.BaseAddress = new Uri(countryOptions.BaseUrl);

            // can add more configuration to the client here
        }).AddHttpMessageHandler<LoggingHandler>();

        return services;
    }
}