using Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Common.Interfaces;

public interface IUserRepository
{
    void Add(User newUser);

    void ChangeEntityContextTracker(object user,EntityState entityState);

    Task<User?> GetUserAndUserRolesByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<User?> GetUserFullInfoByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<User?> GetUserInfoByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<bool> CheckPropertyExistAsync(Expression<Func<User, bool>> expression,List<string>? includeStatements = null);
}
