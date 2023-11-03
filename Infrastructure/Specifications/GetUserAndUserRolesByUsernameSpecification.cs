using Domain;
using Infrastructure.Common;

namespace Infrastructure.Specifications;

public class GetUserAndUserRolesByUsernameSpecification : Specification<User>
{
    public GetUserAndUserRolesByUsernameSpecification(string username)
        : base(user => user.Username == username)
    {
        AddInclude(user => user.UserRoles);
    }
}
