using Application.Common.Interfaces;
using Domain.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Web_Api.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class PrivilegeFilter : Attribute, IAsyncAuthorizationFilter
{
    private readonly UserScope _userScope;
    private readonly IDictionary<UserScope, string> _roleScope = new Dictionary<UserScope, string>();

    public PrivilegeFilter(UserScope userScope)
    {
        _userScope = userScope;
        _roleScope = new Dictionary<UserScope, string>()
        {
            {UserScope.Write,"Write"},
            {UserScope.Read,"Read"},
            {UserScope.Update,"Update"},
            {UserScope.Delete,"Delete"},
            {UserScope.WriteAdmin,"WriteAdmin"},
            {UserScope.ReadAdmin,"ReadAdmin"},
            {UserScope.UpdateAdmin,"UpdateAdmin"},
            {UserScope.DeleteAdmin,"DeleteAdmin"}
        };
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        //Checking if the method have an attribute of allowAnonymous
        bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous) await Task.CompletedTask;

        IAppDbContext? dbContext = context.HttpContext.RequestServices.GetRequiredService<IAppDbContext>();

        // Getting the roles of the current user
        List<string>? currentUserRoles = context.HttpContext.User?.Claims
                                                .Where(claim => claim.Type == ClaimTypes.Role)
                                                .Select(x => x.Value)
                                                .ToList();

        if (currentUserRoles is null) throw new UnauthorizedAccessException();

        if (!await ValidateRolePrivilige(currentUserRoles, dbContext)) throw new UnauthorizedAccessException();
    }

    private async Task<bool> ValidateRolePrivilige(List<string>? roles, IAppDbContext? dbContext)
    {
        if (dbContext is null || roles is null) return false;

        //Returning the count of the existing roles base on the rolenames 
        int rolesCount = await dbContext.Role.Where(role => roles.Contains(role.RoleName))
                                             .Select(role => role.RoleName)
                                             .Distinct()
                                             .CountAsync();
                              
        // if the rolesCount is diferent from the roleNames 
        if (rolesCount != roles.Count) throw new UnauthorizedAccessException();

        // Check if exists a combination with Rolenames pass and the necessary privilege to do the action
        return await dbContext.RolePrivilege
                               .Include(rolePrivilege => rolePrivilege.Role)
                               .Include(rolePrivilege => rolePrivilege.Privilige)
                               .Where(rolePrivilege => roles.Contains(rolePrivilege.Role.RoleName))
                               .AnyAsync(rolePrivilege => rolePrivilege.Privilige.PrivilegeName == _roleScope[_userScope]);
    }
}
