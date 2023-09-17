﻿namespace Domain.Common;

public abstract class AuditableEntity
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}
