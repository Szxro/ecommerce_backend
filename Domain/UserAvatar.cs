﻿using Domain.Common;
using Domain.Common.Owned;

namespace Domain;

public class UserAvatar : AuditableEntity
{
    public Files File { get; set; } = new();

    public int UserId { get; set; }

    public User User { get; set; } = new();
}
