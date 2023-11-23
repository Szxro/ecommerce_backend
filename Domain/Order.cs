using Domain.Common;

namespace Domain;

public class Order : AuditableEntity
{
    public DateTime ArriveDate { get; set; }

    public int ProductQuantity { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = new(); // One to Many (Required)

    public int? ProductId { get; set; } 

    public Product? Product { get; set; } = new(); // One to Many (Non-Required)

    public DateTime OrderDate { get; set; }

    public int? UserPaymentMethodId { get; set; }

    public UserPaymentMethod? UserPaymentMethod { get; set; } = new(); // One to Many (Non-Required)

    public int AddressId { get; set; }

    public Address Address { get; set; } = new(); // One to Many (Required)

    public int? ShippingMethodId { get; set; }

    public ShippingMethod? ShippingMethod { get; set; } = new();

    public int? OrderStatusId { get; set; }

    public OrderStatus? OrderStatus { get; set; } = new(); // One to Many (Non-Required)

    public double OrderTotal { get; set; }
}
