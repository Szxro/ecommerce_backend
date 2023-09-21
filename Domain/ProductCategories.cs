using Domain.Common;

namespace Domain;

public class ProductCategories : AuditableEntity
{
    public int? ProductId { get; set; }

    public Product? Product { get; set; }

    public int? CategoryId { get; set; }

    public Categories? Category { get; set; }
}
