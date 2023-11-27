using Domain.Common;

namespace Domain;

public class Provider : AuditableEntity
{
    public Provider()
    {
        UserPaymentMethods = new HashSet<UserPaymentMethod>();    
    }


    public string ProviderName { get; set; } = string.Empty;

    public int PaymentTypeId { get; set; }

    public PaymentType PaymentType { get; set; } = new();

    public ICollection<UserPaymentMethod> UserPaymentMethods { get; set; }
}
