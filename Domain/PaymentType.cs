using Domain.Common;

namespace Domain;

public class PaymentType : AuditableEntity
{
    public PaymentType()
    {
        UserPaymentMethods = new HashSet<UserPaymentMethod>();
        Providers = new HashSet<Provider>();
    }

    public string TypeName { get; set; } = string.Empty;

    public ICollection<UserPaymentMethod> UserPaymentMethods { get; set; }

    public ICollection<Provider> Providers { get; set; }
}
