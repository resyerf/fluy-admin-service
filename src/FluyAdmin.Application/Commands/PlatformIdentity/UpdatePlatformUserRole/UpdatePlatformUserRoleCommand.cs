using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.PlatformIdentity.UpdatePlatformUserRole;

public sealed record UpdatePlatformUserRoleCommand(Guid PlatformUserId, string NewRole) : ICommand<UpdatePlatformUserRoleResult>;
