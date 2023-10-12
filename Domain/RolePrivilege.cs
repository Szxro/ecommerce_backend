using Domain.Common;

namespace Domain;

public class RolePrivilege : AuditableEntity
{
    public int? RoleId { get; set; }

    public Role Role { get; set; } = new();

    public int? PrivilegeId { get; set; }

    public Privilege Privilige { get; set; } = new();
}
