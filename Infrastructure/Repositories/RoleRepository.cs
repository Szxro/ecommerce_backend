using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<int> CountRolesAsync(List<string> roles,CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new CountRolesSpecification(roles))
                                        .Select(role => role.RoleName)
                                        .Distinct()
                                        .CountAsync(cancellationToken);
    }

    public async Task<Role> GetRoleByRoleName(string roleName,CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new GetRoleByRoleNameSpecification(roleName))
                                        .Select(role => new Role()
                                        {
                                            Id = role.Id,
                                            RoleName = role.RoleName
                                        })
                                        .FirstAsync(cancellationToken);
    }
}
