using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        UpdateAuditableEnttities();
        await _context.SaveChangesAsync(cancellation);
    }

    public void UpdateAuditableEnttities()
    {
        if (_context is null) return;

        var entities = _context.ChangeTracker.Entries<AuditableEntity>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
        // Just the entities that state in context tracker are modified or added

        //Adding the auditable entities
        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
               entity.Entity.CreatedAt = DateTime.UtcNow;
               entity.Entity.ModifiedAt = DateTime.UtcNow;
            }

            if (entity.State == EntityState.Modified)
            {
                entity.Entity.ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}
