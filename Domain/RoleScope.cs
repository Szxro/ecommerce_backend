using Domain.Common;

namespace Domain;

public class RoleScope : AuditableEntity
{
    public int RoleId { get; set; }

    public Role Role { get; set; } = new();

    public int ScopeId { get; set; }

    public Scope Scope { get; set; } = new(); // One to Many (Required)
}
