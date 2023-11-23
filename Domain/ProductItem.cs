using Domain.Common;

namespace Domain;

public class ProductItem : AuditableEntity
{
    public ProductItem()
    {
        ProductFiles = new HashSet<ProductFiles>();
    }

    public Guid SKU { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = new(); // One to Many (Required)

    public ICollection<ProductFiles> ProductFiles { get; set; } // Many to One (Required)
}
