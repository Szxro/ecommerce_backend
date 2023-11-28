using Domain;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    void Add(Role newRole);

    void ChangeTrackerToUnchanged(Role currentRole);

    Task<int> CountRolesAsync(List<string> roles, CancellationToken cancellationToken = default);

    Task<Role?> GetRoleByRoleNameAsync(string roleName, CancellationToken cancellationToken = default);

    Task AddDefaultRolesAndScopeAsync();

    bool CheckHaveAnyData();
}
