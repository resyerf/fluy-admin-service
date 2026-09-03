using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Entities;
using FluyAdmin.Domain.Enums;
using Fluy.SharedKernel.Dispatching;
using Fluy.SharedKernel.Security;

namespace FluyAdmin.Application.Commands.PlatformIdentity.CreatePlatformUser;

public class CreatePlatformUserCommandHandler(IPlatformUserRepository platformUsers, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    : ICommandHandler<CreatePlatformUserCommand, CreatePlatformUserResult>
{
    public async Task<CreatePlatformUserResult> Handle(CreatePlatformUserCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        var emailTaken = await platformUsers.EmailTakenAsync(email, cancellationToken);
        if (emailTaken)
        {
            throw new EmailAlreadyRegisteredException(email);
        }

        var role = Enum.Parse<PlatformRole>(command.Role, ignoreCase: true);
        var passwordHash = passwordHasher.Hash(command.Password);

        var platformUser = PlatformUser.Create(email, command.FullName, passwordHash, role);
        platformUsers.Add(platformUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreatePlatformUserResult(platformUser.Id, platformUser.Email, platformUser.Role.ToString());
    }
}
