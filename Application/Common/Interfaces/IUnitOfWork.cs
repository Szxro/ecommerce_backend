﻿namespace Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellation = default);

    void UpdateAuditableEntities();
}
