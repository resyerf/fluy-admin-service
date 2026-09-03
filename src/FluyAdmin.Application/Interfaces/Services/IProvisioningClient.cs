using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Interfaces.Services;

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

    /// <summary>Solo usado por DemoTenantSeeder (Development) tras BootstrapTenantAsync.</summary>
    Task SeedDemoDataAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
