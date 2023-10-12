using Domain;
using System.Linq.Expressions;

namespace Application.Common.Interfaces;

public interface IRoleRepository
{
    void Add(Role newRole);

    void ChangeTrackerToUnchanged(Role currentRole);

    Task<Role?> GetBy(Expression<Func<Role,bool>> expression,ICollection<string>? includes = null);

    Task<ICollection<string>> GetUserRoleNames(ICollection<int?> rolesId);
}
