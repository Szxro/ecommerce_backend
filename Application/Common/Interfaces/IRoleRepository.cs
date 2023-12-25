using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    void Add(Role newRole);

    void ChangeEntityContextTracker(object currentRole,EntityState entityState);

    Task<int> CountRolesAsync(List<string> roles, CancellationToken cancellationToken = default);

    Task<Role?> GetRoleByRoleNameAsync(string roleName, CancellationToken cancellationToken = default);

    Task AddDefaultRolesAndScopeAsync();

    bool CheckHaveAnyData();
}
