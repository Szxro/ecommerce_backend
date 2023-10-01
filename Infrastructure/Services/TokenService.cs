using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain;
using Infrastructure.Options.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IDateService _date;
    private readonly IRoleRepository _role;

    public TokenService(
        IOptions<JwtOptions> options,
        IDateService date,
        IRoleRepository role)
    {
        _jwtOptions = options.Value;
        _date = date;
        _role = role;
    }

    public async Task<string> GenerateToken(User currentUser,double tokenLifeTime = 10.00) // by default is going 10 min to expire
    {
        //Initialiazing the Handler
        JwtSecurityTokenHandler handler = new();

        //Encoding the Key
        byte[] key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

        //Making the Token Descriptor
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Issuer = _jwtOptions.ValidIssuer,
            Audience = _jwtOptions.ValidAudience,
            Subject = await GenerateClaims(currentUser),
            Expires = DateTime.UtcNow.AddMinutes(tokenLifeTime),
            SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha512)
            // NotBefore => future
        };

        //Creating the token
        SecurityToken token = handler.CreateToken(tokenDescriptor);

        //Returning the token in string
        return handler.WriteToken(token);
    }

    private async Task<ClaimsIdentity> GenerateClaims(User user) 
    {
        List<int?> rolesId= user.UserRoles.Select(userRole => userRole.RoleId).ToList(); // returning the ids associated with the user

        ICollection<Role> userRoles = await _role.GetUserRoles(rolesId); // returning the role associated with that id

        if (!userRoles.Any())
        {
            throw new NotFoundException("The roles of the user was not found");
        }

        ClaimsIdentity userClaims = new ClaimsIdentity(new[]
            {
                new Claim("uid",$"{user.Id}"),
                new Claim(JwtRegisteredClaimNames.Name,user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) // Unique Guid by Users
            }.Union(userRoles.Select(role => new Claim(ClaimTypes.Role, role.RoleName))).ToList());

        return userClaims;
    }
}
