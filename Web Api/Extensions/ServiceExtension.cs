using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Web_Api.Middleware;

namespace Web_Api.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            //Adding the Authentication Header
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                //Adding some descriptions 
                Description = "Standard Authorization header using the Bearer Scheme(\"Bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            //Lastly adding the operation filter
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        return services;
    }


    public static IServiceCollection RegisterMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ExceptionMiddleware>();

        return services;
    }
}
