using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.Tenants.ActivateTenant;

/// <summary>CLAUDE.md §5/§32 ("Activar tenant") — reactiva el acceso operativo (ej. tras reactivar el pago).</summary>
public sealed record ActivateTenantCommand(Guid TenantId) : ICommand<ActivateTenantResult>;
