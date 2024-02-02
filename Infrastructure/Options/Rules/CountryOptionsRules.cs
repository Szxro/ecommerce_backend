using FluentValidation;
using Infrastructure.Options.Country;

namespace Infrastructure.Options.Rules;

public class CountryOptionsRules : AbstractValidator<CountryOptions>
{
    public CountryOptionsRules()
    {
        RuleFor(x => x.BaseUrl)
            .NotEmpty()
            .WithName("The {PropertyName} can't be empty");

        RuleFor(x => x.BaseUrl)
              .Must(baseUrl => Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri? _))
              .When(baseUrl => !string.IsNullOrWhiteSpace(baseUrl.BaseUrl));
    }
}
