using Domain.Common;

namespace Domain
{
    public class Product : AuditableEntity
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategory>();
            ProductFiles = new HashSet<ProductFile>();
            Orders = new HashSet<Order>();
        }

        public string ProductName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public int ProductDiscount { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } // Many to Many (Nullable)

        public ICollection<ProductFile> ProductFiles { get; set; } // Many to One (Required)

        public ICollection<Order> Orders { get; set; } // Many to One (Nullable)
    }
}
