using Domain;

namespace Infrastructure.Specifications.Roles;

public class GetRoleByRoleName : Specification<Role>
{
    public GetRoleByRoleName(string roleName)
        : base(role => role.RoleName == roleName)
    {
    }
}
