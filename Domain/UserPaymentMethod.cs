using Domain.Common;

namespace Domain;

public class UserPaymentMethod : AuditableEntity
{
    public UserPaymentMethod()
    {
        Orders = new HashSet<Order>();
    }

    public int UserId { get; set; }

    public User User { get; set; } = new(); // One to Many (Required)

    public int? ProviderId { get; set; }

    public Provider? Provider { get; set; } = new(); // One to Many (Non-Required)

    public int? PaymentTypeId { get; set; }

    public PaymentType? PaymentType { get; set; } = new(); // One to Many (Non-Required)

    public int AccountNumber { get; set; }

    public bool  IsDefault { get; set; }

    public ICollection<Order> Orders { get; set; }
}
