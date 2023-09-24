using MediatR;
using Domain;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mapping;

namespace Application.Features.Users.Commands.Create;

public record CreateUserCommand : IRequest<Unit>
{
    public string Fullname { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IUserRepository _user;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IUserRepository user,
        IRoleRepository role)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _user = user;
    }
    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!CheckPasswordEquality(request.Password, request.ConfirmPassword)) throw new PasswordException("The password and confirm password must be equal");

        //Generating UserHash and UserSalt
        string userHash = _passwordService.GenerateUserHashAndSalt(request.Password,out byte[] userSalt);

        //Mapping from CreateUserCommand to User (Manual) 
        User newUser = request.ToUser(userHash,userSalt);

        _user.Add(newUser);

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }

    private bool CheckPasswordEquality(string password,string confirmPassword) => password.Equals(confirmPassword);
}
