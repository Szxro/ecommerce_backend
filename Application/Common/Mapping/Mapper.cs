using Application.Features.Users.Commands.Create;
using Domain;

namespace Application.Common.Mapping;

public static class Mapper
{
    public static User ToUser(this CreateUserCommand newUser,string userHash, byte[] userSalt)
    {
        return new User()
        {
            Username = newUser.Username,
            Email = newUser.Email,
            FullName = newUser.Fullname,
            UserHash = new UserHash() {HashValue = userHash },
            UserSalt = new UserSalt() { SaltValue = Convert.ToHexString(userSalt) }
        };
    }
}
