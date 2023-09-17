using Domain.Common;

namespace Domain;

public class UserSalt : AuditableEntity
{
    public string SaltValue { get; set; } = string.Empty;

    public  int UserId { get; set; }

    public User User { get; set; } = new();
}
