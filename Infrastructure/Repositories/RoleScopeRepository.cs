using Application.Common.Interfaces;
using Domain;
using Domain.Common.Enum;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleScopeRepository : GenericRepository<RoleScope>, IRoleScopeRepository
{
    public RoleScopeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> CombinationRoleNameAndPrivilegeAsync(List<string> roles,
                                                      IDictionary<UserScope, string> rolesScope,
                                                      UserScope userScope,
                                                      CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new CombinationRoleNameAndScopeSpecification(roles, rolesScope))
                                        .AnyAsync(roleScope => roleScope.Scope!.ScopeName == rolesScope[userScope],cancellationToken);
    }
}
