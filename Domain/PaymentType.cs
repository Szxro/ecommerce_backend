using Domain.Common;

namespace Domain;

public class PaymentType : AuditableEntity
{
    public PaymentType()
    {
        UserPaymentMethods = new HashSet<UserPaymentMethod>();    
    }

    public string TypeName { get; set; } = string.Empty;

    public ICollection<UserPaymentMethod> UserPaymentMethods { get; set; }
}
