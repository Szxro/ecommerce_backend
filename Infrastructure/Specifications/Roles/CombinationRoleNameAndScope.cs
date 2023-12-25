using Domain;
using Domain.Common.Enum;

namespace Infrastructure.Specifications.Roles;

public class CombinationRoleNameAndScope : Specification<RoleScope>
{
    public CombinationRoleNameAndScope(
        List<string> roles, IDictionary<UserScope, string> rolesScope)
        : base(roleScope => roles.Contains(roleScope.Role!.RoleName))
    {
        AddInclude(roleScope => roleScope.Scope!);
        AddInclude(roleScope => roleScope.Role!);
    }
}
