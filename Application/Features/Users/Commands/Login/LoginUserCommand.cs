using MediatR;
using Domain.Common.Response;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Domain.Guards;
using Domain.Guards.Extensions;
using Domain;

namespace Application.Features.Users.Commands.Login;

public record LoginUserCommand(string username,string password) : IRequest<TokenResponse>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _password;
    private readonly IUserActivityRepository _userActivityRepository;
    private readonly IDateService _dateService;

    public LoginUserCommandHandler(IUserRepository userRepository,
                                   ITokenService tokenService,
                                   IPasswordService password,
                                   IUserActivityRepository userActivityRepository,
                                   IDateService dateService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _password = password;
        _userActivityRepository = userActivityRepository;
        _dateService = dateService;
    }
    public async Task<TokenResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? currentUser = await _userRepository.GetUserFullInfoByUsernameAsync(request.username, cancellationToken);

        Guard.Against.Null(currentUser, nameof(currentUser), $"The username {request.username} was not found");

        string? currentUserHash = currentUser.UserHash?.HashValue;

        Guard.Against.NullOrEmpty(currentUserHash, nameof(currentUserHash), "Invalid Hash");

        byte[]? currentUserSalt = Convert.FromHexString(currentUser.UserSalt!.SaltValue);

        Guard.Against.Null(currentUserSalt, nameof(currentUserSalt), "Invalid Salt");

        if (!IsPasswordValid(request.password, currentUserHash, currentUserSalt))
        {
            throw new PasswordException("The password is invalid, try again");
        }

        if (!await IsUserAlreadyRegisterInUserActivityAsync(request.username))
        {
            await _userActivityRepository.InsertUserActivity(currentUser, _dateService.NowUTC, cancellationToken);

            return new TokenResponse(_tokenService.GenerateToken(currentUser)); ;
        }

        await _userActivityRepository.UpdateUserLoggedDateInByUsername(request.username, _dateService.NowUTC);

        //Generating the token and returning it as a response
        return new TokenResponse(_tokenService.GenerateToken(currentUser));
    }

    private bool IsPasswordValid(string password,string userHash,byte[] userSalt)
    {
       return  _password.CompareUserHash(password, userHash, userSalt);
    }

    private async Task<bool> IsUserAlreadyRegisterInUserActivityAsync(string username)
    {
        return await _userActivityRepository.IsUserAlreadyRegisterByUsernameAsync(username); 
    }
}