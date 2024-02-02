using Domain.Guards;
using Domain.Guards.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Options.Validators;

public class FluentOptionsValidator<TOptions> : IValidateOptions<TOptions>
    where TOptions : class

{
    // IServiceScopeFactory create IScopedServices instances 
    private readonly IServiceScopeFactory _scopeFactory;

    public FluentOptionsValidator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        
        // Because the validator service is register as singleton and need to use a scope service need to use IServiceScopeFactory or IServiceProvider
        using var scope = _scopeFactory.CreateScope();
        // Its not a good idea to use a scope service in a singleton service (its consider an anti-pattern) 

        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

        var optionsName = typeof(TOptions).Name;

        Guard.Against.Null(options, nameof(options), $"The {optionsName} cant be null");

        ValidationResult result = validator.Validate(options);

        if (result.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var errors = result.Errors.Select(x => $"Validation failed to {x.PropertyName} with the Error Message {x.ErrorMessage}").ToList();

        return ValidateOptionsResult.Fail(errors);
    }
}
