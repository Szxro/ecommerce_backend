using Domain;
using Infrastructure.Common;

namespace Infrastructure.Specifications;

public class CountRolesSpecification : Specification<Role>
{
    public CountRolesSpecification(List<string> roles)
        : base(role => roles.Contains(role.RoleName))
    {
    }
}
