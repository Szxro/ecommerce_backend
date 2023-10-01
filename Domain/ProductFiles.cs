using Domain.Common;
using Domain.Common.Owned;

namespace Domain;

public class ProductFiles : AuditableEntity
{
    public Files File { get; set; } = new();

    public int ProductId { get; set; }

    public Product Product { get; set; } = new();
}
