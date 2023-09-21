using Domain.Common;

namespace Domain;

public class Categories : AuditableEntity
{
    public Categories()
    {
        ProductCategories = new HashSet<ProductCategories>();    
    }

    public string CategoryName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<ProductCategories> ProductCategories { get; set; }
}
