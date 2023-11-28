using Application.Common.Interfaces;
using Domain;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleRepository(AppDbContext context,IUnitOfWork unitOfWork) : base(context)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddDefaultRolesAndScopeAsync()
    {
        // Defaults roles and scopes
        ICollection<Role> roles = new HashSet<Role>()
            {
               new Role(){
                   RoleName = "User",
                   Description = "Normal User",
                   Scopes = new HashSet<RoleScope>()
                   {
                       new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "Write",
                                Description = "User Write"
                           }
                       },
                       new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "Read",
                                Description = "User Read"
                           }
                       },
                       new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "Update",
                                Description = "User Update"
                           }
                       },
                       new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "Delete",
                                Description = "User Delete"
                           }
                       }
                   }
               },
               new Role(){
                   RoleName = "Admin",
                   Description = "Super User",
                   Scopes = new HashSet<RoleScope>()
                   {
                        new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "WriteAdmin",
                                Description = "Admin Write"
                           }
                       },
                        new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "ReadAdmin",
                                Description = "Admin Read"
                           }
                       },
                        new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "UpdateAdmin",
                                Description = "Admin Update"
                           }
                       },
                        new RoleScope(){
                           Scope = new Scope()
                           {
                                ScopeName = "DeleteAdmin",
                                Description = "Admin Delete"
                           }
                       }
                   }
               }
            };

        _context.Role.AddRange(roles);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> CountRolesAsync(List<string> roles,CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new CountRolesSpecification(roles))
                                        .Select(role => role.RoleName)
                                        .Distinct()
                                        .CountAsync(cancellationToken);
    }

    public async Task<Role?> GetRoleByRoleNameAsync(string roleName,CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(new GetRoleByRoleNameSpecification(roleName))
                                        .Select(role => new Role()
                                        {
                                            Id = role.Id,
                                            RoleName = role.RoleName
                                        })
                                        .FirstOrDefaultAsync(cancellationToken);
    }
}
