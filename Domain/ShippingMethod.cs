using Domain.Common;

namespace Domain;

public class ShippingMethod : AuditableEntity
{
    public ShippingMethod()
    {
        Orders = new HashSet<Order>();    
    }

    public string MethodName { get; set; } = string.Empty;

    public double Price { get; set; }

    public ICollection<Order> Orders { get; set; }
}
