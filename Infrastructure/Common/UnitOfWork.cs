using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IDateService _dateService;

    public UnitOfWork(AppDbContext context,
                      IDateService dateService)
    {
        _context = context;
        _dateService = dateService;
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
               entity.Entity.CreatedAt = _dateService.NowUTC;
               entity.Entity.ModifiedAt = _dateService.NowUTC;
            }

            if (entity.State == EntityState.Modified)
            {
                entity.Entity.ModifiedAt = _dateService.NowUTC;
            }
        }
    }
}
