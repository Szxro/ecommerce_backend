using Domain;
using Infrastructure.Common;

namespace Infrastructure.Specifications;

public class GetRoleByRoleNameSpecification : Specification<Role>
{
    public GetRoleByRoleNameSpecification(string roleName) 
        : base(role => role.RoleName == roleName)
    {
    }
}
