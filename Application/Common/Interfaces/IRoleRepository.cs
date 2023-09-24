using Domain;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByRoleName(string roleName);
}
