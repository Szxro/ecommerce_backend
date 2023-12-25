using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Registering the mediatr and pipeline behaviors => Trasient
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); // <,> => mean that all that enter is allow
            options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(QueryCachingPipelineBehavior<,>));
        });

        //Registering the validations => Transient
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}