using Domain.Common;

namespace Domain;

public class UserRoles : AuditableEntity
{
    public int? UserId { get; set; }

    public User? User { get; set; } = new();

    public int? RoleId { get; set; }

    public Role? Role { get; set; } = new();
}
