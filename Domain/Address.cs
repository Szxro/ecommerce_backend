using Domain.Common;

namespace Domain;

public class Address : AuditableEntity
{
    public Address()
    {
        UserAddresses = new HashSet<UserAddress>();
        Orders = new HashSet<Order>();
    }

    public int StreetNumber { get; set; }

    public string AddressLine1 { get; set; } = string.Empty;

    public string AddressLine2 { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;

    public int PostalCode { get; set; }

    public int? CountryId { get; set; }

    public Country? Country { get; set; } = new(); // One to Many (Non-required)

    public ICollection<UserAddress> UserAddresses { get; set; }

    public ICollection<Order> Orders { get; set; }
}
