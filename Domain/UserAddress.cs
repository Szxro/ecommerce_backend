using Domain.Common;

namespace Domain;

public class UserAddress : AuditableEntity
{
    public int UserId { get; set; }

    public User User { get; set; } = new();

    public int AddressId { get; set; }

    public Address Address { get; set; } = new();

    public bool IsDefault { get; set; }
}
