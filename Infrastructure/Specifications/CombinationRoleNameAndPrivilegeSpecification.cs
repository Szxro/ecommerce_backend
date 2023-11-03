using Domain;
using Domain.Common.Enum;
using Infrastructure.Common;

namespace Infrastructure.Specifications;

public class CombinationRoleNameAndPrivilegeSpecification : Specification<RolePrivilege>
{
    public CombinationRoleNameAndPrivilegeSpecification(List<string> roles,
                                                        IDictionary<UserScope, string> rolesScope)
        :base(rolePrivilege => roles.Contains(rolePrivilege.Role.RoleName))
    {
        AddInclude(rolePrivilege => rolePrivilege.Privilige);
        AddInclude(rolePrivilege => rolePrivilege.Role);
    }
}
