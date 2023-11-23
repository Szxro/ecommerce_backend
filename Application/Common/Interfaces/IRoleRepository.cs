using Domain;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    void Add(Role newRole);

    void ChangeTrackerToUnchanged(Role currentRole);

    Task<int> CountRolesAsync(List<string> roles, CancellationToken cancellationToken = default);

    Task<Role> GetRoleByRoleName(string roleName, CancellationToken cancellationToken = default);

    Task AddDefaultRolesAndScope();
}
