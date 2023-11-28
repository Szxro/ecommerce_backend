using Domain.Common.Enum;

namespace Application.Common.Interfaces;

public interface IRoleScopeRepository
{
    Task<bool> CombinationRoleNameAndPrivilegeAsync(List<string> roles,
                                                       IDictionary<UserScope, string> rolesScope,
                                                       UserScope userScope,
                                                       CancellationToken cancellationToken = default);
}
