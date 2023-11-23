using Domain.Common;

namespace Domain;

public class OrderStatus : AuditableEntity
{
    public OrderStatus()
    {
        Orders = new HashSet<Order>();
    }
    public string StatusName { get; set; } = string.Empty;

    public ICollection<Order> Orders { get; set; }
}
