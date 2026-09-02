using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Tenants.ActivateTenant;

/// <summary>CLAUDE.md §5/§32 ("Activar tenant") — reactiva el acceso operativo (ej. tras reactivar el pago).</summary>
public record ActivateTenantCommand(Guid TenantId) : ICommand<ActivateTenantResult>;

public record ActivateTenantResult(Guid TenantId, string Status);
