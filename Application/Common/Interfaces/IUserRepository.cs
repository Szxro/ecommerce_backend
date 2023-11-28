using Domain;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    void Add(User newUser);

    void ChangeTrackerToUnchanged(User user);

    Task<User?> GetUserClaimsByUsernameAsync(string username,CancellationToken cancellationToken = default);

    Task<User?> GetUserAndUserRolesByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
