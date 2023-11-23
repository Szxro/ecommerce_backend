using Application.Common.Interfaces;
using Domain.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
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

        //Getting an instance of the following repositories
        IRoleScopeRepository? rolePrivilege = context.HttpContext.RequestServices.GetRequiredService<IRoleScopeRepository>();

        IRoleRepository roleRepository = context.HttpContext.RequestServices.GetRequiredService<IRoleRepository>();

        // Getting the roles of the current user
        List<string>? currentUserRoles = context.HttpContext.User?.Claims
                                                .Where(claim => claim.Type == ClaimTypes.Role)
                                                .Select(x => x.Value)
                                                .ToList();

        if (currentUserRoles is null) throw new UnauthorizedAccessException();

        if (!await ValidateRolePrivilige(currentUserRoles, rolePrivilege,roleRepository)) throw new UnauthorizedAccessException();
    }

    private async Task<bool> ValidateRolePrivilige(List<string> roles,
                                                   IRoleScopeRepository? rolePrivilege,
                                                   IRoleRepository? roleRepository)
    {
        if (rolePrivilege is null || roleRepository is null || !roles.Any()) return false;

        //Returning the count of the existing roles base on the rolenames 
        int rolesCount = await roleRepository.CountRolesAsync(roles);

        // if the rolesCount is diferent from the roleNames 
        if (rolesCount != roles.Count) throw new UnauthorizedAccessException();

        return await rolePrivilege.CombinationRoleNameAndPrivilege(roles, _roleScope, _userScope);
    }
}
