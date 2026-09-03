using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.PlatformIdentity.CreatePlatformUser;

/// <summary>CLAUDE.md §5 (Platform Admin) — alta de personal interno de FLUY. Role: SuperAdmin|BillingOps|Support|ReadOnly.</summary>
public sealed record CreatePlatformUserCommand(string Email, string FullName, string Password, string Role)
    : ICommand<CreatePlatformUserResult>;
