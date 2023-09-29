using Domain;
using System.Linq.Expressions;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetBy(Expression<Func<User, bool>> expression, ICollection<string>? includes = null);

    void Add(User newUser);

    void ChangeTrackerToUnchanged(User user);
}
