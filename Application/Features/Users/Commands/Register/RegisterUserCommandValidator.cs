using FluentValidation;

namespace Application.Features.Users.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        //In the first failure it stop and continue the next validation
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(user => user.Email).NotEmpty().NotNull().WithMessage("The {PropertyName} cant be empty");

        RuleFor(user => user.Email).EmailAddress().WithMessage("The {PropertyName} need to be a valid email");

        RuleFor(user => user.Username).NotEmpty().NotNull().WithMessage("The {PropertyName} cant be empty");
    }
}
