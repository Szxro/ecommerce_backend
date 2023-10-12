using Application.Common.Interfaces;
using Domain.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Web_Api.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class PrivilegeFilter : Attribute, IAsyncAuthorizationFilter
{
    private readonly UserPrivilege _userPrivilege;
    private readonly IDictionary<UserPrivilege, string> _rolePrivilege = new Dictionary<UserPrivilege, string>();

    public PrivilegeFilter(UserPrivilege userPrivilege)
    {
        _userPrivilege = userPrivilege;
        _rolePrivilege = new Dictionary<UserPrivilege, string>()
        {
            {UserPrivilege.Write,"Write"},
            {UserPrivilege.Read,"Read"},
            {UserPrivilege.Update,"Update"},
            {UserPrivilege.Delete,"Delete"},
            {UserPrivilege.WriteAdmin,"WriteAdmin"},
            {UserPrivilege.ReadAdmin,"ReadAdmin"},
            {UserPrivilege.UpdateAdmin,"UpdateAdmin"},
            {UserPrivilege.DeleteAdmin,"DeleteAdmin"}
        };
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        //Checking if the method have an attribute of allowAnonymous
        bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

        if (allowAnonymous) await Task.CompletedTask;

        IAppDbContext? dbContext = context.HttpContext.RequestServices.GetRequiredService<IAppDbContext>();

        // Getting the roles that was place in the httpContext
        object? roles = context.HttpContext.Items["roles"];

        if (roles is null) throw new UnauthorizedAccessException();

        if (!await ValidateRolePrivilige(roles, dbContext)) throw new UnauthorizedAccessException();
    }

    private async Task<bool> ValidateRolePrivilige(object? roles, IAppDbContext? dbContext)
    {
        if (dbContext is null || roles is null) return false;

        if (roles is List<string> roleNames)
        {
            //Returning the count of the existing roles base on the rolenames 
            int rolesCount = await dbContext.Role.Where(role => roleNames.Contains(role.RoleName))
                                             .Select(role => role.RoleName)
                                             .Distinct()
                                             .CountAsync();
                              
            // if the rolesCount is diferent from the roleNames 
            if (rolesCount != roleNames.Count) throw new UnauthorizedAccessException();

            // Check if exists a combination with Rolenames pass and the necessary privilege to do the action
            return await dbContext.RolePrivilege
                                         .Include(rolePrivilege => rolePrivilege.Role)
                                         .Include(rolePrivilege => rolePrivilege.Privilige)
                                         .Where(rolePrivilege => roleNames.Contains(rolePrivilege.Role.RoleName))
                                         .AnyAsync(rolePrivilege => rolePrivilege.Privilige.PrivilegeName == _rolePrivilege[_userPrivilege]);
        }
        return false;
    }
}
