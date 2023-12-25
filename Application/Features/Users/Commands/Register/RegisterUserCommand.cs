using MediatR;
using Domain;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mapping;

namespace Application.Features.Users.Commands.Register;

public record RegisterUserCommand : IRequest<string>
{
    public string Fullname { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IUserRepository _user;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IUserRepository user)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _user = user;
    }
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await IsUserUsernameAlreadyRegister(request.Username))
        {
            throw new Exception("The username already is registered");
        }

        if (await IsUserEmailAlreadyRegister(request.Email))
        {
            throw new Exception("The email already is registered");
        }

        if (!CheckPasswordEquality(request.Password, request.ConfirmPassword)) throw new PasswordException("The password and confirm password must be equal");

        //Generating UserHash and UserSalt
        string userHash = _passwordService.GenerateUserHashAndSalt(request.Password,out byte[] userSalt);

        //Mapping from CreateUserCommand to User (Manual) 
        User newUser = request.ToUser(userHash,userSalt);

        _user.Add(newUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newUser.Username;
    }

    private bool CheckPasswordEquality(string password, string confirmPassword)
    {
        return password.Equals(confirmPassword);
    }

    private async Task<bool> IsUserUsernameAlreadyRegister(string username)
    {
        return await _user.CheckPropertyExistAsync(x => x.Username == username);
    }

    private async Task<bool> IsUserEmailAlreadyRegister(string email)
    {
        return await _user.CheckPropertyExistAsync(x => x.Email == email);
    }
}
