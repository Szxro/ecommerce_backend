using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UserRoleRepository : GenericRepository<UserRoles>, IUserRoleRepository
{
    public UserRoleRepository(AppDbContext context) : base(context)
    {
    }
}
