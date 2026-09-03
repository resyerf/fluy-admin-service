using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.PlatformIdentity.ActivatePlatformUser;

public sealed record ActivatePlatformUserCommand(Guid PlatformUserId) : ICommand<SetPlatformUserStatusResult>;
