using Application.Common.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        return await _context.SaveChangesAsync(cancellation);
    }
}
