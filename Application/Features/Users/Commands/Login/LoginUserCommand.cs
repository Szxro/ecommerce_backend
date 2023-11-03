using MediatR;
using Domain.Common.Response;
using Application.Common.Interfaces;
using Domain;
using Application.Common.Exceptions;

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
        User? currentUser = await _user.GetUserClaimsByUsername(request.username,cancellationToken);

        if (currentUser is null) throw new NotFoundException($"The username <{request.username}> was not found");

        string? currentUserHash = currentUser.UserHash?.HashValue;

        if (string.IsNullOrWhiteSpace(currentUserHash)) throw new Exception("Invalid hash");

        byte[]? currentUserSalt = Convert.FromHexString(currentUser.UserSalt!.SaltValue);

        if (currentUserSalt is null) throw new Exception("Invalid salt");

        bool isPasswordCorrect = _password.CompareUserHash(request.password,currentUserHash,currentUserSalt);

        if (!isPasswordCorrect) throw new PasswordException("Incorrect password, try again");

        //Generating the token and returning it as a response
        return new TokenResponse(_tokenService.GenerateToken(currentUser));
    }
}