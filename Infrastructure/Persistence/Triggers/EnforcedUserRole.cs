using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Triggers;

public class EnforcedUserRole : IBeforeSaveTrigger<User>
{
    private readonly AppDbContext _context;
    private readonly IRoleRepository _roleRepository;
    private readonly IDateService _dateService;
    private const string _userRole = "User";

    public EnforcedUserRole(AppDbContext context,
                            IRoleRepository roleRepository,
                            IDateService dateService)
    {
        _context = context;
        _roleRepository = roleRepository;
        _dateService = dateService;
    }
    public async Task BeforeSave(ITriggerContext<User> context, CancellationToken cancellationToken)
    {
        Role? currentRole = await _roleRepository.GetBy(role => role.RoleName == _userRole);

        if (currentRole is null) throw new NotFoundException($"The role with the rolename <{_userRole}> was not found");

        UserRoles newUserRoles = new()
        {
            User = context.Entity,
            Role = currentRole,
            CreatedAt = _dateService.NowUTC, 
            ModifiedAt = _dateService.NowUTC,
        };

        // Have to add the properties added by the unit of work because these its trigger before the user is saved

        if (context.ChangeType == ChangeType.Added)
        {
            _context.UserRoles.Add(newUserRoles);        
        }

        _context.Entry(newUserRoles.Role).State = EntityState.Unchanged;
        
        await Task.CompletedTask;
    }
}
