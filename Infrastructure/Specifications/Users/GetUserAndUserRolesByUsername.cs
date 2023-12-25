using Domain;

namespace Infrastructure.Specifications.Users;

public class GetUserAndUserRolesByUsername : Specification<User>
{
    public GetUserAndUserRolesByUsername(string username)
        : base(user => user.Username == username)
    {
        AddInclude(user => user.UserRoles);
    }
}
