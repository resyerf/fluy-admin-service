namespace FluyAdmin.Application.Common.Interfaces;

/// <summary>fluy-service envía el email de activación directamente (CODE.md §9.22) — esta respuesta solo informa si el envío fue exitoso.</summary>
public record BootstrapTenantResponse(Guid MasterUserId, bool ActivationEmailSent);

/// <summary>
/// Cliente hacia la API administrativa interna de fluy-service (CODE.md §9.5-9.6). Es la única
/// dirección "admin → tenant" permitida: nunca se escribe directamente en el schema "tenant"
/// desde fluy-admin-service, porque crear un User correctamente exige pasar por las invariantes
/// del agregado que solo Fluy.Domain/Fluy.Application conocen.
/// </summary>
public interface IProvisioningClient
{
    Task<BootstrapTenantResponse> BootstrapTenantAsync(
        Guid tenantId, string masterEmail, string masterFullName, CancellationToken cancellationToken = default);
}
