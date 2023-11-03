using Domain;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User currentUser, double tokenLifeTime = 10.00);

    bool ValidateTokenLifetime(string token);
}
