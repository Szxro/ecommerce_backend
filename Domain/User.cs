using Domain.Common;

namespace Domain
{
    public class User : AuditableEntity
    {
        public User()
        {
            UserRoles = new HashSet<UserRoles>();
            UserOrders = new HashSet<Order>();
            UserShippingInfos = new HashSet<UserShippingInfo>();
        }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public DateTime LockOutEnd { get; set; } = new DateTime(1999,01,01,01,00,00);// Default 1999-01-01 01:00:00

        public bool LockOutEnable { get; set; } = true;  // Default true

        public int AccessFailedCount { get; set; } = 0; // Default 0

        public Avatar? Avatar { get; set; } // One to One (Required)

        public UserHash? UserHash { get; set; } // One to One (Required)

        public UserSalt? UserSalt { get; set; } // One to One (Required)

        public ICollection<UserRoles> UserRoles { get; set; } // One to Many (Nullable)

        public ICollection<Order> UserOrders { get; set; } // One to Many (Nullable)

        public ICollection<UserShippingInfo> UserShippingInfos { get; set; } // One to Many (Required)
    }
}