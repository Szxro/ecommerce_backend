using Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Loading the configuration of the entities for Assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("datetime2");

        base.ConfigureConventions(configurationBuilder);
    }

    public DbSet<User> User => Set<User>();

    public DbSet<UserHash> UserHash => Set<UserHash>();

    public DbSet<UserSalt> UserSalt => Set<UserSalt>(); 

    public DbSet<UserAvatar> UserAvatar => Set<UserAvatar>();

    public DbSet<Address> Address => Set<Address>();

    public DbSet<Country> Country => Set<Country>();

    public DbSet<UserAddress> UserAddress => Set<UserAddress>();
 
    public DbSet<Role> Role => Set<Role>();

    public DbSet<UserRoles> UserRoles => Set<UserRoles>();

    public DbSet<Scope> Scope => Set<Scope>();

    public DbSet<RoleScope> RoleScope => Set<RoleScope>();

    public DbSet<Product> Product => Set<Product>();

    public DbSet<ProductItem> ProductItem => Set<ProductItem>();

    public DbSet<ProductFiles> ProductFiles => Set<ProductFiles>(); 

    public DbSet<Category> Category => Set<Category>();

    public DbSet<ProductCategory> ProductCategory => Set<ProductCategory>();

    public DbSet<Order> Order => Set<Order>();

    public DbSet<UserPaymentMethod> UserPaymentMethods => Set<UserPaymentMethod>();

    public DbSet<PaymentType> PaymentType => Set<PaymentType>();

    public DbSet<Provider> Provider => Set<Provider>();

    public DbSet<ShippingMethod> ShippingMethod => Set<ShippingMethod>();

    public DbSet<OrderStatus> OrderStatus => Set<OrderStatus>();
}
