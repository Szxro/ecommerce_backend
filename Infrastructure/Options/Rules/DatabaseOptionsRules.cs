using FluentValidation;
using Infrastructure.Options.Database;

namespace Infrastructure.Options.Rules;

public class DatabaseOptionsRules : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsRules()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithMessage("The {PropertyName} can't be empty");

        RuleFor(x => x.MaxRetryCount)
            .NotNull()
            .WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.MaxRetryCount)
            .GreaterThan(0)
            .WithMessage("The {PropertyName} must be greater than 0");

        RuleFor(x => x.CommandTimeout)
            .NotNull().
            WithMessage("The {PropertyName} can't be null");


        RuleFor(x => x.CommandTimeout)
            .GreaterThan(0)
            .WithMessage("The {PropertyName} must be greater than 0");

        RuleFor(x => x.EnableDetailedErrors)
            .NotNull()
            .WithMessage("The {PropertyName} can't be empty");

        RuleFor(x => x.EnableSensitiveDataLogging)
            .NotNull()
            .WithMessage("The {PropertyName} can't be empty");
    }
}
