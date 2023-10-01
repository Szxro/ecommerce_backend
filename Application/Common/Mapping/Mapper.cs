using Application.Features.Users.Commands.Register;
using Domain;

namespace Application.Common.Mapping;

public static class Mapper
{
    public static User ToUser(this RegisterUserCommand newUser,string userHash, byte[] userSalt)
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
