using Domain.Common;
using Domain.Common.Owned;

namespace Domain;

public class ProductFiles : AuditableEntity
{
    public Files File { get; set; } = new();

    public int ProductItemId { get; set; }

    public ProductItem ProductItem { get; set; } = new();
}
