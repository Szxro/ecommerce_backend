using Domain.Common;

namespace Domain;

public class Category : AuditableEntity
{
    public Category()
    {
        ProductCategories = new HashSet<ProductCategory>();    
    }

    public string CategoryName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<ProductCategory> ProductCategories { get; set; }
}
