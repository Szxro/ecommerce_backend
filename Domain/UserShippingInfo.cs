using Domain.Common;

namespace Domain;

public class UserShippingInfo : AuditableEntity
{
    public string Address { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public int UserId { get; set; }

    public User User { get; set; } = new();
}
