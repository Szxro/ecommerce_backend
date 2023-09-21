using Domain.Common;

namespace Domain
{
    public class Product : AuditableEntity
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategories>();
            ProductFiles = new HashSet<ProductFiles>();
            Orders = new HashSet<Order>();
        }

        public string ProductName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public int ProductDiscount { get; set; }

        public ICollection<ProductCategories> ProductCategories { get; set; } // Many to Many (Nullable)

        public ICollection<ProductFiles> ProductFiles { get; set; } // Many to One (Required)

        public ICollection<Order> Orders { get; set; } // Many to One (Nullable)
    }
}
