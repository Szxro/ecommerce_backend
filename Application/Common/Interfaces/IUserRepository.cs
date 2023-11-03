using Domain;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    void Add(User newUser);

    void ChangeTrackerToUnchanged(User user);

    Task<User?> GetUserClaimsByUsername(string username,CancellationToken cancellationToken = default);

    Task<User?> GetUserAndUserRolesByUsername(string username, CancellationToken cancellationToken = default);
}
