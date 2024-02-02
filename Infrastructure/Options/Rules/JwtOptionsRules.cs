using FluentValidation;
using Infrastructure.Options.JWT;

namespace Infrastructure.Options.Rules;

public class JwtOptionsRules : AbstractValidator<JwtOptions>
{
    public JwtOptionsRules()
    {
        RuleFor(x => x.ValidAudience)
            .NotNull()
            .WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.ValidAudience)
            .Must(issuer => Uri.TryCreate(issuer, UriKind.Absolute, out Uri? _))
            .When(options => !string.IsNullOrEmpty(options.ValidIssuer));

        RuleFor(x => x.ValidIssuer)
            .NotNull()
            .WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.ValidIssuer)
            .Must(issuer => Uri.TryCreate(issuer, UriKind.Absolute, out Uri? _))
            .When(options => !string.IsNullOrEmpty(options.ValidIssuer));

        RuleFor(x => x.SecretKey)
            .NotNull()
            .WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.ExpiresIn)
            .NotNull()
            .WithMessage("The {PropertyName} can't be null");

        RuleFor(x => x.ExpiresIn)
            .GreaterThan(0)
            .WithMessage("The {PropertyName} must be greater than 0");
    }
}
