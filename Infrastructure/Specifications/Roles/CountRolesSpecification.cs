using Domain;

namespace Infrastructure.Specifications.Roles;

public class CountRolesSpecification : Specification<Role>
{
    public CountRolesSpecification(List<string> roles)
        : base(role => roles.Contains(role.RoleName))
    {
    }
}
