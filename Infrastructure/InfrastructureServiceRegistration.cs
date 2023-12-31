﻿using Application.Common.DelegatingHandlers;
using Application.Common.Interfaces;
using Infrastructure.Common;
using Infrastructure.Interceptors;
using Infrastructure.Options.Country;
using Infrastructure.Options.Database;
using Infrastructure.Options.Hash;
using Infrastructure.Options.JWT;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       IWebHostEnvironment environment)
    {
        //Adding the Configuration
        services.ConfigureOptions<DatabaseOptionsSetup>();
        services.ConfigureOptions<HashOptionsSetup>();
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<CountryOptionsSetup>();

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

            if (environment.IsDevelopment()) // can check if the enviroment is in production
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IAppDbInitializer, AppDbInitializer>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IRoleScopeRepository, RoleScopeRepository>();
        services.AddTransient<IUserRoleRepository, UserRoleRepository>();
        services.AddTransient<IPasswordService, PasswordService>();
        services.AddTransient<IDateService, DateService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IPaymentTypeRepository, PaymentTypeRepository>();
        services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();
        services.AddTransient<IShippingMethodRepository, ShippingMethodRepository>();
        services.AddTransient<ICountryRepository, CountryRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IUserActivityRepository, UserActivityRepository>();
        services.AddSingleton<ICacheService, CacheService>();

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

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        //Creating a scope instance
        using var scope = serviceProvider.CreateScope();

        //Creating a service instance with the scope instance
        var initializer = scope.ServiceProvider.GetRequiredService<IAppDbInitializer>();

        //Running the methods
        await initializer.ConnectAsync();

        await initializer.MigrateAsync();

        await initializer.SeedAsync();
    }

    public static IServiceCollection AddInfrastructureAuthentication(this IServiceCollection services,IConfiguration config)
    {
        //Adding the Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            //Validation parameters
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JWTOptions:ValidIssuer"],
                ValidAudience = config["JWTOptions:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["JWTOptions:SecretKey"]!)),
                ClockSkew = TimeSpan.FromSeconds(5) // must override (validating time) (default one is 5 min)
            };
        });

        return services;
    }
}