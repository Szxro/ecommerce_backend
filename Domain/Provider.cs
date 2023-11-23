using Domain.Common;

namespace Domain;

public class Provider : AuditableEntity
{
    public Provider()
    {
        UserPaymentMethods = new HashSet<UserPaymentMethod>();    
    }

    public string ProviderName { get; set; } = string.Empty;

    public ICollection<UserPaymentMethod> UserPaymentMethods { get; set; }
}
