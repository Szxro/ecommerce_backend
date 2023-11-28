using Domain.Exceptions;
using Application.Common.Interfaces;
using Domain;
using Infrastructure.Options.JWT;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Guards;
using Domain.Guards.Extensions;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IDateService _date;
    private readonly IRoleRepository _role;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public TokenService(
        IOptions<JwtOptions> options,
        IDateService date,
        IRoleRepository role)
    {
        _jwtOptions = options.Value;
        _date = date;
        _role = role;
        _tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = false, // to not have a time exception 
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.ValidIssuer,
            ValidAudience = _jwtOptions.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecretKey))
        };
    }

    public string GenerateToken(User currentUser,double tokenLifeTime = 10.00) // by default is going 10 min to expire
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
            Subject = GenerateClaims(currentUser),
            Expires = DateTime.UtcNow.AddMinutes(tokenLifeTime),
            SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha512)
            // NotBefore => future
        };

        //Creating the token
        SecurityToken token = handler.CreateToken(tokenDescriptor);

        //Returning the token in string
        return handler.WriteToken(token);
    }

    public bool ValidateTokenLifetime(string token)
    {
        JwtSecurityTokenHandler handler = new();

        ClaimsPrincipal tokenClaims = handler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);

        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        {
            bool isTokenAlgorithmValid = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

            if (!isTokenAlgorithmValid) throw new TokenException("Invalid Token Algorithm");
        }

        string? tokenExpiryStamp = tokenClaims.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp)?.Value;

        if (tokenExpiryStamp is null) throw new TokenException("Invalid Token Stamp");

        DateTime tokenExpiryDate = _date.TimeStampToUTCDate(long.Parse(tokenExpiryStamp));

        return tokenExpiryDate > DateTime.UtcNow;
    }

    private ClaimsIdentity GenerateClaims(User user) 
    {
        List<string>? userRoles = user.UserRoles.Select(role => role.Role.RoleName).ToList();

        Ensure.Against.NotNullOrEmpty(userRoles, nameof(userRoles), "The roles of the user was not found");

        ClaimsIdentity userClaims = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,$"{user.Id}"), // Unique by Users
                new Claim(JwtRegisteredClaimNames.Name,user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }.Union(userRoles.Select(roleNames => new Claim(ClaimTypes.Role,roleNames))).ToList());

        return userClaims;
    }
}
