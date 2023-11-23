using Domain.Common;

namespace Domain;

public class Country : AuditableEntity
{
    public Country()
    {
        Addresses = new HashSet<Address>();     
    }

    public string CountryName { get; set; } = string.Empty;

    public ICollection<Address> Addresses { get; set; }
}
