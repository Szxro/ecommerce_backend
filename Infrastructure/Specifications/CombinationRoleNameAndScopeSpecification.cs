using Domain;
using Domain.Common.Enum;
using Infrastructure.Common;

namespace Infrastructure.Specifications;

public class CombinationRoleNameAndScopeSpecification : Specification<RoleScope>
{
    public CombinationRoleNameAndScopeSpecification(List<string> roles,
                                                        IDictionary<UserScope, string> rolesScope)
        :base(roleScope => roles.Contains(roleScope.Role!.RoleName))
    {
        AddInclude(roleScope => roleScope.Scope!);
        AddInclude(roleScope => roleScope.Role!);
    }
}
