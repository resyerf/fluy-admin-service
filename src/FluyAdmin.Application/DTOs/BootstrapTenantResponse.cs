namespace FluyAdmin.Application.DTOs;

/// <summary>fluy-service envía el email de activación directamente (CODE.md §9.22) — esta respuesta solo informa si el envío fue exitoso.</summary>
public record BootstrapTenantResponse(Guid MasterUserId, bool ActivationEmailSent);
