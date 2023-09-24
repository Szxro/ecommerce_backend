using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _context.User.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.User.Include(user => user.UserRoles).FirstAsync(user => user.Username == username);
    }
}
