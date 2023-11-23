using Domain.Common;

namespace Domain;

public  class Role : AuditableEntity
{
    public Role()
    {
        UserRoles = new HashSet<UserRoles>();
        Scopes = new HashSet<RoleScope>();
    }

    public string RoleName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<UserRoles> UserRoles { get; set; }

    public ICollection<RoleScope> Scopes { get; set; }
}
