using FluentValidation;
using Infrastructure.Options.Hash;

namespace Infrastructure.Options.Rules;

public class HashOptionsRules : AbstractValidator<HashOptions>
{
    public HashOptionsRules()
    {
        RuleFor(x => x.Iterations)
            .NotNull()
            .WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.Iterations)
            .GreaterThan(0)
            .WithMessage("The {PropertyName} must be greater than 0");

        RuleFor(x => x.KeySize)
            .NotNull()
            .WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.KeySize)
            .GreaterThan(0)
            .WithMessage("The {PropertyName} must be greater than 0");
    }
}
