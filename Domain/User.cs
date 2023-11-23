using Domain.Common;

namespace Domain
{
    public class User : AuditableEntity
    {
        public User()
        {
            UserRoles = new HashSet<UserRoles>();
            Orders = new HashSet<Order>();
            Addresses = new HashSet<UserAddress>();
            PaymentMethods = new HashSet<UserPaymentMethod>();
        }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public DateTime LockOutEnd { get; set; } = new DateTime(1999,01,01,01,00,00);// Default 1999-01-01 01:00:00

        public bool LockOutEnable { get; set; } = true;  // Default true

        public int AccessFailedCount { get; set; } = 0; // Default 0

        public UserAvatar? UserAvatar { get; set; } // One to One (Required)

        public UserHash? UserHash { get; set; } // One to One (Required)

        public UserSalt? UserSalt { get; set; } // One to One (Required)

        public ICollection<UserRoles> UserRoles { get; set; } // One to Many (Nullable)

        public ICollection<UserAddress> Addresses { get; set; } // Many to One (Required)

        public ICollection<UserPaymentMethod> PaymentMethods { get; set; }

        public ICollection<Order> Orders { get; set; } // One to Many (Required)
    }
}