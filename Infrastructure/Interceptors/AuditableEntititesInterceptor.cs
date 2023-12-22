using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors;

public sealed class AuditableEntititesInterceptor : SaveChangesInterceptor
{
    private readonly IDateService _dateService;

    public AuditableEntititesInterceptor(IDateService dateService)
    {
        _dateService = dateService;
    }

    // Is going to trigger in the process of the saving the changes 
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext context)
    {
        // Getting all the entities that inherance from the auditable entities abstract class and are added or modified
        var entities = context.ChangeTracker.Entries<AuditableEntity>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList();

        foreach (EntityEntry<AuditableEntity> entry in entities)
        {
            // checking the current state of the entry (and updating some properties)
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(entry, nameof(AuditableEntity.CreatedAt), _dateService.NowUTC);
                SetCurrentPropertyValue(entry, nameof(AuditableEntity.ModifiedAt), _dateService.NowUTC);
            }

            if (entry.State == EntityState.Modified)
            {
                SetCurrentPropertyValue(entry, nameof(AuditableEntity.ModifiedAt), _dateService.NowUTC);
            }
        }
    }

    private void SetCurrentPropertyValue(
        EntityEntry entity,
        string propertyName,
        DateTime dateTime
        ) => entity.Property(propertyName).CurrentValue = dateTime;
}
