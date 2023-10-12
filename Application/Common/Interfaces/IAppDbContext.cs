using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<RolePrivilege> RolePrivilege { get;}

    DbSet<Role> Role { get; }

    DbSet<Privilege> Privileges { get; }
}
