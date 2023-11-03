using Domain.Common.Enum;

namespace Application.Common.Interfaces;

public interface IRolePrivilegeRepository
{
    Task<bool> CombinationRoleNameAndPrivilege(List<string> roles,
                                                       IDictionary<UserScope, string> rolesScope,
                                                       UserScope userScope,
                                                       CancellationToken cancellationToken = default);
}
