using Application.Common.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Extensions;

namespace Infrastructure.Persistence;

public class AppDbInitializer : IAppDbInitializer
{
    private readonly ILogger<AppDbInitializer> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context;

    public AppDbInitializer(
        ILogger<AppDbInitializer> logger,
        IUnitOfWork unitOfWork,
        AppDbContext context)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task ConnectAsync()
    {
        try 
        {
            await _context.Database.CanConnectAsync(); // Determines whether the database is avaliable
        } catch (Exception ex)
        {
            _logger.ConnectDatabaseError(_context.Database.ProviderName, ex.Message);
        }
    }

    public async Task EnsuredDatabaseCreated()
    {
        try 
        {
            await _context.Database.EnsureCreatedAsync(); // check the database have any table and the database exists

        } catch (Exception ex)
        {
            _logger.DatabaseCreatedError(_context.Database.ProviderName, ex.Message);
        }
    }

    public async Task MigrateAsync()
    {
        try
        {
            await _context.Database.MigrateAsync(); // Looking for any new migration to do it automatically the api start
        }
        catch (Exception ex)
        {
            _logger.MigrateDatabaseError(_context.Database.ProviderName, ex.Message);

            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();

        } catch (Exception ex)
        {
            _logger.SeedDatabaseError(ex.Message);

            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Role.Any())
        {
            // Defaults roles and privileges
            ICollection<Role> roles = new HashSet<Role>()
            {
               new Role(){
                   RoleName = "User",
                   Description = "Normal User",
                   RolePrivileges = new HashSet<RolePrivilege>()
                   {
                       new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "Write",
                                Description = "User Write"
                           }
                       },
                       new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "Read",
                                Description = "User Read"
                           }
                       },
                       new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "Update",
                                Description = "User Update"
                           }
                       },
                       new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "Delete",
                                Description = "User Delete"
                           }
                       }
                   }
               },
               new Role(){
                   RoleName = "Admin",
                   Description = "Super User",
                   RolePrivileges = new HashSet<RolePrivilege>()
                   {
                        new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "WriteAdmin",
                                Description = "Admin Write"
                           }
                       },
                        new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "ReadAdmin",
                                Description = "Admin Read"
                           }
                       },
                        new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "UpdateAdmin",
                                Description = "Admin Update"
                           }
                       },
                        new RolePrivilege(){
                           Privilige = new Privilege()
                           {
                                PrivilegeName = "DeleteAdmin",
                                Description = "Admin Delete"
                           }
                       }
                   }
               }
            };

            _context.Role.AddRange(roles);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
