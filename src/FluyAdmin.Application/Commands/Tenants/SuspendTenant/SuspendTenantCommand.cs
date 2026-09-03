using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.Tenants.SuspendTenant;

/// <summary>CLAUDE.md §5/§32 ("Suspender tenant") — corta el acceso operativo, independiente del estado de Subscription (CODE.md §9.13).</summary>
public sealed record SuspendTenantCommand(Guid TenantId) : ICommand<SuspendTenantResult>;
