using Domain.Common;

namespace Domain;

public class Avatar : AuditableEntity
{
    public Files File { get; set; } = new();

    public int UserId { get; set; }

    public User User { get; set; } = new();
}
