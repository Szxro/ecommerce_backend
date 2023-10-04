using Application.Common.Interfaces;
using Infrastructure.Common;
using Infrastructure.Options.Database;
using Infrastructure.Options.Hash;
using Infrastructure.Options.JWT;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            // Adding Dependencies 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAppDbInitializer, AppDbInitializer>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IDateService, DateService>();
            services.AddTransient<ITokenService, TokenService>();

            return services;
        }

        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            //Creating a scope instance
            using var scope = app.Services.CreateScope();

            //Creating a service instance with the scope instance
            var initializer = scope.ServiceProvider.GetRequiredService<IAppDbInitializer>();

            await initializer.InitializeAsync();

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
}