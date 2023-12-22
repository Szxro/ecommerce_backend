using Domain.Common;

namespace Domain;

public class UserActivity : AuditableEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = new();

    public DateTime LoggedDate { get; set; }

    public bool IsOffline { get; set; }
}
