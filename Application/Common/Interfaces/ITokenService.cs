using Domain;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> GenerateToken(User currentUser, double tokenLifeTime = 10.00);

    (List<string> Roles, string? tokenExpiryStamp) ValidateAndReturnClaims(string token);
    //(type nameType,type nameType) => this is call a tuple
}
