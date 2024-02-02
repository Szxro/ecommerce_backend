using Application.Common.Interfaces;
using Infrastructure.Common;
using Infrastructure.Options.Validators;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddFluentValidator<TOptions>(this IServiceCollection services)
       where TOptions : class
    {
        // The default validator in this extension method is FluentOptionsValidator
        services.AddSingleton<IValidateOptions<TOptions>>(
            serviceProvider => new FluentOptionsValidator<TOptions>(serviceProvider.GetRequiredService<IServiceScopeFactory>()));

        return services;
    }

    public static IServiceCollection AddRepositoriesAndServices(this IServiceCollection services)
    {
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

        return services;
    }

    public static IServiceCollection AddInfrastructureAuthentication(this IServiceCollection services, IConfiguration config)
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
}
