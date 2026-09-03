using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Enums;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.PlatformIdentity.UpdatePlatformUserRole;

public class UpdatePlatformUserRoleCommandHandler(IPlatformUserRepository platformUsers, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdatePlatformUserRoleCommand, UpdatePlatformUserRoleResult>
{
    public async Task<UpdatePlatformUserRoleResult> Handle(UpdatePlatformUserRoleCommand command, CancellationToken cancellationToken)
    {
        var platformUser = await platformUsers.GetByIdAsync(command.PlatformUserId, cancellationToken)
            ?? throw new PlatformUserNotFoundException(command.PlatformUserId);

        var role = Enum.Parse<PlatformRole>(command.NewRole, ignoreCase: true);
        platformUser.ChangeRole(role);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdatePlatformUserRoleResult(platformUser.Id, platformUser.Role.ToString());
    }
}
