using Domain.Common;

namespace Domain;

public class Scope : AuditableEntity
{
    public Scope()
    {
        RoleScope = new HashSet<RoleScope>();
    }

    public string ScopeName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<RoleScope> RoleScope { get; set; }
}
