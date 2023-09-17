using Domain.Common;

namespace Domain;

public class UserHash : AuditableEntity
{
    public string HashValue { get; set; } = string.Empty;

    public int UserId { get; set; }

    public User User { get; set; } = new();
}
