using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.PlatformIdentity.DeactivatePlatformUser;

public class DeactivatePlatformUserCommandHandler(IPlatformUserRepository platformUsers, IUnitOfWork unitOfWork)
    : ICommandHandler<DeactivatePlatformUserCommand, SetPlatformUserStatusResult>
{
    public async Task<SetPlatformUserStatusResult> Handle(DeactivatePlatformUserCommand command, CancellationToken cancellationToken)
    {
        var platformUser = await platformUsers.GetByIdAsync(command.PlatformUserId, cancellationToken)
            ?? throw new PlatformUserNotFoundException(command.PlatformUserId);

        platformUser.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SetPlatformUserStatusResult(platformUser.Id, platformUser.Status.ToString());
    }
}
