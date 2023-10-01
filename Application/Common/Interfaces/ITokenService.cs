using Domain;

namespace Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> GenerateToken(User currentUser, double tokenLifeTime = 10.00);
}
