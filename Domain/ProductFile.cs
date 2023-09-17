﻿using Domain.Common;

namespace Domain;

public class ProductFile : AuditableEntity
{
    public Files File { get; set; } = new();

    public int ProductId { get; set; }

    public Product Product { get; set; } = new();
}
