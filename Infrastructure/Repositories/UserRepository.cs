using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserAndUserRolesByUsername(string username, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new GetUserAndUserRolesByUsernameSpecification(username))
                                        .Select(user => new User()
                                        {
                                            Id = user.Id,
                                            Username = user.Username,
                                            UserRoles = user.UserRoles.Select(userRoles => new UserRoles() { RoleId = userRoles.RoleId }).ToList(),
                                        })
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetUserClaimsByUsername(string username, CancellationToken cancellationToken = default)
    {
        return await _context.User
                                  .Where(user => user.Username == username)
                                  .Include(user => user.UserHash)
                                  .Include(user => user.UserSalt)
                                  .Include(user => user.UserRoles)
                                    .ThenInclude(user => user.Role)
                                   .Select(user => new User()
                                   {
                                        Id = user.Id,
                                        Username = user.Username,
                                        Email = user.Email,
                                        UserRoles = user.UserRoles
                                                        .Select(userRoles => new UserRoles()
                                                        {
                                                           Role = new()
                                                           {
                                                              RoleName = userRoles.Role!.RoleName
                                                           }
                                                        }).ToList(),
                                        UserSalt = new UserSalt()
                                        {
                                            SaltValue = user.UserSalt!.SaltValue
                                        },
                                        UserHash = new UserHash()
                                        {
                                            HashValue = user.UserHash!.HashValue
                                        }
                                   })
                                   .AsNoTracking()
                                   .AsSplitQuery()
                                  .FirstOrDefaultAsync(cancellationToken);
                                  
    }
}
