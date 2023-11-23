using Domain.Common;

namespace Domain
{
    public class Product : AuditableEntity
    {
        public Product()
        {
            ProductCategories = new HashSet<ProductCategory>();
            ProductItems = new HashSet<ProductItem>();
            Orders = new HashSet<Order>();
        }

        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ProductImagePath { get; set; } = string.Empty;

        public ICollection<ProductCategory> ProductCategories { get; set; } // One to Many (Required)

        public ICollection<ProductItem> ProductItems { get; set; } 

        public ICollection<Order> Orders { get; set; }
    }
}
