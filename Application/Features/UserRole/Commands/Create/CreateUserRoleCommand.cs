using Application.Common.Interfaces;
using Domain;
using Domain.Guards;
using Domain.Guards.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        User? currentUser = await _userRepository.GetUserAndUserRolesByUsernameAsync(request.username);

        Guard.Against.Null(currentUser, nameof(currentUser), $"The username <{request.username}> was not found");

        Role? currentRole = await _roleRepository.GetRoleByRoleNameAsync(request.rolename);

        Guard.Against.Null(currentRole, nameof(currentRole), $"The role with the rolename <{request.rolename}> was not found");

        if (IsRoleAlreadyInTheUser(currentUser, currentRole.Id, currentRole.RoleName))
        {
            throw new ArgumentException($"The user already have that role <{currentRole.RoleName}>");
        };

        UserRoles newUserRoles = new()
        {
            User = currentUser,
            Role = currentRole,
        };

        // Changing tracking to unchanged
        _userRepository.ChangeEntityContextTracker(newUserRoles.User,EntityState.Unchanged);
        _roleRepository.ChangeEntityContextTracker(newUserRoles.Role,EntityState.Unchanged);

        _userRoleRepository.Add(newUserRoles);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private bool IsRoleAlreadyInTheUser(User currentUser,int roleId,string currentRoleName)
    {
        return currentUser.UserRoles.Any(role => role.RoleId == roleId);
    }
}
