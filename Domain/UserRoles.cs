using Domain.Common;

namespace Domain;

public class UserRoles : AuditableEntity
{
    // if the user or the role is deleted the relation is going to be deleted too
    public int UserId { get; set; }

    public User User { get; set; } = new();

    public int RoleId { get; set; }

    public Role Role { get; set; } = new();
}
