using Domain.Common;

namespace Domain;

public class Order : AuditableEntity
{
    public DateTime ArriveDate { get; set; }

    public int ProductQuantity { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = new();

    public int ProductId { get; set; }

    public Product Product { get; set; } = new();
}
