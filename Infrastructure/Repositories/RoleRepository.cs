﻿using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ICollection<Role>> GetUserRoles(ICollection<int?> rolesId)
    {
        return await _context.Role.Where(role => rolesId.Contains(role.Id) && rolesId != null).ToListAsync();
    }
}
