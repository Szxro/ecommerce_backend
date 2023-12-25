using Domain;

namespace Infrastructure.Specifications.Users;

public class GetUserInfoByUsername : Specification<User>
{
    public GetUserInfoByUsername(string username) : base(user => user.Username == username)
    {
          
    }
}
