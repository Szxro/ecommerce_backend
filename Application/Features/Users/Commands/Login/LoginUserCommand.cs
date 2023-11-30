using MediatR;
using Domain.Common.Response;
using Application.Common.Interfaces;
using Domain;
using Application.Common.Exceptions;
using Domain.Guards;
using Domain.Guards.Extensions;

namespace Application.Features.Users.Commands.Login;

public record LoginUserCommand(string username,string password) : IRequest<TokenResponse>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResponse>
{
    private readonly IUserRepository _user;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _password;

    public LoginUserCommandHandler(IUserRepository user,
                                   ITokenService tokenService,
                                   IPasswordService password)
    {
        _user = user;
        _tokenService = tokenService;
        _password = password;
    }
    public async Task<TokenResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? currentUser = await _user.GetUserClaimsByUsernameAsync(request.username,cancellationToken);

        Ensure.Against.Null(currentUser, nameof(currentUser), $"The username {request.username} was not found");

        string? currentUserHash = currentUser.UserHash?.HashValue;

        Ensure.Against.NullOrEmpty(currentUserHash, nameof(currentUserHash), "Invalid Hash");

        byte[]? currentUserSalt = Convert.FromHexString(currentUser.UserSalt!.SaltValue);

        Ensure.Against.Null(currentUserSalt, nameof(currentUserSalt), "Invalid Salt");

        IsPasswordValid(request.password,currentUserHash,currentUserSalt);

        //Generating the token and returning it as a response
        return new TokenResponse(_tokenService.GenerateToken(currentUser));
    }

    private void IsPasswordValid(string password,string userHash,byte[] userSalt)
    {
        bool isPasswordCorrect = _password.CompareUserHash(password, userHash, userSalt);

        if (!isPasswordCorrect) throw new PasswordException("Incorrect password, try again");
    }
}