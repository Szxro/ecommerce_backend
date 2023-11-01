using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    public string? GetCurrentUsername()
    {
        return _accessor?.HttpContext?.User?.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Name)?.Value;
    }

    public List<string>? GetCurrentUserRoles()
    {
        return _accessor?.HttpContext?.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
    }
}
