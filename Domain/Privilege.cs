using Domain.Common;

namespace Domain;

public class Privilege : AuditableEntity
{
    public Privilege()
    {
        RolePrivileges = new HashSet<RolePrivilege>();
    }

    public string PrivilegeName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<RolePrivilege> RolePrivileges { get; set; }
}
