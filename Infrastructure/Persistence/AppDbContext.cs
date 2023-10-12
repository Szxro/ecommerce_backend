using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext,IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Loading the configuration of the entities for Assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> User => Set<User>();

    public DbSet<UserHash> UserHash => Set<UserHash>();

    public DbSet<UserSalt> UserSalt => Set<UserSalt>();

    public DbSet<ShippingInfo> ShippingInfo => Set<ShippingInfo>(); 

    public DbSet<Avatar> Avatar => Set<Avatar>();

    public DbSet<Role> Role => Set<Role>();

    public DbSet<UserRoles> UserRoles => Set<UserRoles>();

    public DbSet<Privilege> Privileges => Set<Privilege>();

    public DbSet<RolePrivilege> RolePrivilege => Set<RolePrivilege>();

    public DbSet<Product> Product => Set<Product>();

    public DbSet<Categories> Categories => Set<Categories>();

    public DbSet<ProductCategories> ProductCategories => Set<ProductCategories>();

    public DbSet<ProductFiles> ProductFiles => Set<ProductFiles>(); 

    public DbSet<Order> Order => Set<Order>();
}
