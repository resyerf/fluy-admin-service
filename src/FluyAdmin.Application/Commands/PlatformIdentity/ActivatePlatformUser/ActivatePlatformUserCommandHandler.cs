using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.PlatformIdentity.ActivatePlatformUser;

public class ActivatePlatformUserCommandHandler(IPlatformUserRepository platformUsers, IUnitOfWork unitOfWork)
    : ICommandHandler<ActivatePlatformUserCommand, SetPlatformUserStatusResult>
{
    public async Task<SetPlatformUserStatusResult> Handle(ActivatePlatformUserCommand command, CancellationToken cancellationToken)
    {
        var platformUser = await platformUsers.GetByIdAsync(command.PlatformUserId, cancellationToken)
            ?? throw new PlatformUserNotFoundException(command.PlatformUserId);

        platformUser.Activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SetPlatformUserStatusResult(platformUser.Id, platformUser.Status.ToString());
    }
}
