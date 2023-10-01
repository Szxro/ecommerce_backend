using FluentValidation;

namespace Application.Features.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(prop => prop.username).NotEmpty().WithMessage("The {PropertyName} cant be empty");

        RuleFor(prop => prop.password).NotEmpty().WithMessage("The {PropertyName} cant be empty");
    }
}
