using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.PlatformIdentity.DeactivatePlatformUser;

public sealed record DeactivatePlatformUserCommand(Guid PlatformUserId) : ICommand<SetPlatformUserStatusResult>;
