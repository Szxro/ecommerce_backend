using Application.Common.Interfaces;
using Domain;
using Domain.Guards;
using Domain.Guards.Extensions;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByUsername;

public record GetUserByUsernameQuery(string username) : ICachedQuery<User>
{
    public string Key => $"user-{username}"; 

    public TimeSpan? ExpirationTime => null; // to implement the default expiration time located in the CachedService
}

public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByUsernameQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<User> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        User? currentUser = await _userRepository.GetUserInfoByUsernameAsync(request.username, cancellationToken);

        Guard.Against.Null(currentUser, nameof(currentUser), "The user by the username provided was not found");

        return currentUser; 
    }
}
