using Domain;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByRoleName(string roleName);

    void Add(Role newRole);

    void ChangeTrackerToUnchanged(Role currentRole);
}
