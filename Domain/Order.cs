using Domain.Common;

namespace Domain;

public class Order : AuditableEntity
{
    // Nullable UserId

    public int? UserId { get; set; }

    public User? User { get; set; }

    public int? ProductId { get; set; }

    public Product? Product { get; set; } = new();

    public DateTime ArriveDate { get; set; }
}
