using FluentValidation;

namespace Application.Features.UserRole.Commands.Create;

public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
{
    public CreateUserRoleCommandValidator()
    {

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(userRole => userRole.username).NotEmpty().NotNull().WithMessage("The {PropertyName} cant be empty");

        RuleFor(userRole => userRole.rolename).NotEmpty().WithMessage("The {PropertyName} cant be a valid email");
    }
}
