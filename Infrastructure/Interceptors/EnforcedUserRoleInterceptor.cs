using Application.Common.Interfaces;
using Domain;
using Domain.Common;
using Domain.Guards;
using Domain.Guards.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors;

public sealed class EnforcedUserRoleInterceptor : SaveChangesInterceptor
{
    private readonly IDateService _dateService;
    private const string _userRole = "User";

    public EnforcedUserRoleInterceptor(IDateService dateService)
    {
        _dateService = dateService;
    }

    // These will act as a trigger when a user is saving in the database
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            EnforcedUserRole(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    // When using the 
    private void EnforcedUserRole(DbContext context)
    {
        //Just the Entities that represents the User class
        var users = context.ChangeTracker.Entries<AuditableEntity>().Where(x => x.Entity is User && x.State == EntityState.Added).ToList();

        if (!users.Any()) return;

        // When the using the pass context need to use sync operations with it
        Role? userRole = context.Set<Role>().Where(x => x.RoleName == _userRole).FirstOrDefault();

        Guard.Against.Null(userRole,nameof(userRole), $"The role with the rolename {_userRole} was not found");

        List<UserRoles> userRoles = users.Select(entry => new UserRoles()
        { 
            User = (User)entry.Entity, // casting the entities
            Role = userRole,
            ModifiedAt = _dateService.NowUTC,
            CreatedAt = _dateService.NowUTC,
        }).ToList();
   
        userRoles.Select(x => context.Entry(x.Role).State == EntityState.Unchanged).ToList();
          
        context.Set<UserRoles>().AddRange(userRoles); // When using the context register the interceptor service as scoped

        context.SaveChanges();
    }
}
