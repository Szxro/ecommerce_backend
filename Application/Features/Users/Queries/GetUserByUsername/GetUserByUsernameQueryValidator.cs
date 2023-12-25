using FluentValidation;

namespace Application.Features.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserByUsernameQueryValidator()
    {
        RuleFor(x => x.username).NotNull().WithMessage("The {PropertyName} cant be null");

        RuleFor(x => x.username).NotEmpty().WithMessage("The {PropertyName} cant be empty");
    }
}
