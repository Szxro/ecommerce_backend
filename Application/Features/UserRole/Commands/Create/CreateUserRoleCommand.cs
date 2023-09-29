using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain;
using MediatR;

namespace Application.Features.UserRole.Commands.Create;

public record CreateUserRoleCommand(string username,string rolename) : IRequest<Unit>;

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserRoleCommandHandler(IUserRepository userRepository,
                                        IUserRoleRepository userRoleRepository,
                                        IRoleRepository roleRepository,
                                        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        User? currentUser = await _userRepository.GetBy(x => x.Username == request.username,new List<string> { "UserRoles" });

        if (currentUser is null) throw new NotFoundException($"The username <{request.username}> was not found");

        Role? currentRole = await _roleRepository.GetBy(x => x.RoleName == request.rolename);

        if (currentRole is null) throw new NotFoundException($"The role with the rolename <{request.rolename}> was not found");

        if (currentUser.UserRoles.Any(role => role.RoleId == currentRole.Id)) throw new ArgumentException($"The user already have that role <{currentRole.RoleName}>");

        UserRoles newUserRoles = new()
        {
           User = currentUser,
           Role = currentRole,
        };

        // Changing tracking to unchanged
        _userRepository.ChangeTrackerToUnchanged(newUserRoles.User);
        _roleRepository.ChangeTrackerToUnchanged(newUserRoles.Role);
        
        _userRoleRepository.Add(newUserRoles);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
