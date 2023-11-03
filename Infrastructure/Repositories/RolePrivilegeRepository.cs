using Application.Common.Interfaces;
using Domain;
using Domain.Common.Enum;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RolePrivilegeRepository : GenericRepository<RolePrivilege>, IRolePrivilegeRepository
{
    public RolePrivilegeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> CombinationRoleNameAndPrivilege(List<string> roles,
                                                      IDictionary<UserScope, string> rolesScope,
                                                      UserScope userScope,
                                                      CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new CombinationRoleNameAndPrivilegeSpecification(roles, rolesScope))
                                        .AnyAsync(rolePrivilige => rolePrivilige.Privilige.PrivilegeName == rolesScope[userScope],cancellationToken);
    }
}
