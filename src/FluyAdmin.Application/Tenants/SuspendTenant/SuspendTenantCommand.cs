using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Tenants.SuspendTenant;

/// <summary>CLAUDE.md §5/§32 ("Suspender tenant") — corta el acceso operativo, independiente del estado de Subscription (CODE.md §9.13).</summary>
public record SuspendTenantCommand(Guid TenantId) : ICommand<SuspendTenantResult>;

public record SuspendTenantResult(Guid TenantId, string Status);
