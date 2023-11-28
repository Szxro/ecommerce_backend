using Application.Features.Users.Commands.Register;
using Domain;
using Domain.Common.Request;

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

    public static HashSet<Country> ToCountry(this ICollection<CountryRequest> countryRequest)
    {
        return countryRequest.Select(country => new Country() { CountryName = country.Name.Common }).ToHashSet();
    }
}
