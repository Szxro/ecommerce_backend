using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Specifications.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserAndUserRolesByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new GetUserAndUserRolesByUsername(username))
                                        .Select(user => new User()
                                        {
                                            Id = user.Id,
                                            Username = user.Username,
                                            UserRoles = user.UserRoles.Select(userRoles => new UserRoles() { RoleId = userRoles.RoleId }).ToList(),
                                        })
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetUserFullInfoByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.User
                             .Where(user => user.Username == username)
                             .Include(user => user.UserSalt)
                             .Include(user => user.UserHash)
                             .Include(user => user.UserRoles)
                               .ThenInclude(userRole => userRole.Role)
                              .Select(user => new User()
                              {
                                  Id = user.Id,
                                  Username = user.Username,
                                  Email = user.Email,
                                  FullName = user.FullName,
                                  LockOutEnd = user.LockOutEnd,
                                  LockOutEnable = user.LockOutEnable,
                                  AccessFailedCount = user.AccessFailedCount,
                                  UserRoles = user.UserRoles
                                                  .Select(userRole => new UserRoles()
                                                  {
                                                      Role = new Role()
                                                      {
                                                          RoleName = userRole.Role.RoleName
                                                      }
                                                  })
                                                  .ToList(),
                                  UserSalt = new UserSalt()
                                  {
                                      SaltValue = user.UserSalt!.SaltValue
                                  },
                                  UserHash = new UserHash()
                                  {
                                      HashValue = user.UserHash!.HashValue
                                  }
                              })
                             .AsSplitQuery() // need to install the sql efcore nugget
                             .AsNoTracking()
                             .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetUserInfoByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new GetUserInfoByUsername(username))
                                        .Select(user => new User()
                                        {
                                             Id = user.Id,
                                             Username = user.Username,
                                             Email = user.Email,
                                             FullName = user.FullName,
                                             LockOutEnable = user.LockOutEnable,
                                             LockOutEnd = user.LockOutEnd,
                                             AccessFailedCount = user.AccessFailedCount,
                                         })
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(cancellationToken);
    }
}
