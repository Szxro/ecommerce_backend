using Domain.Common;

namespace Domain;

public class ProductCategory : AuditableEntity
{
    public int? ProductId { get; set; }

    public Product? Product { get; set; }

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
}
