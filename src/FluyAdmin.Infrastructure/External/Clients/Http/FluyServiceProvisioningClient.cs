using System.Net.Http.Json;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;

namespace FluyAdmin.Infrastructure.External.Clients.Http;

public class FluyServiceProvisioningClient(HttpClient httpClient) : IProvisioningClient
{
    private record BootstrapRequest(string MasterEmail, string MasterFullName);
    private record BootstrapResponse(Guid MasterUserId, bool ActivationEmailSent);

    public async Task<BootstrapTenantResponse> BootstrapTenantAsync(
        Guid tenantId, string masterEmail, string masterFullName, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            $"api/internal/provisioning/tenants/{tenantId}/bootstrap",
            new BootstrapRequest(masterEmail, masterFullName),
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<BootstrapResponse>(cancellationToken)
            ?? throw new InvalidOperationException("fluy-service devolvió una respuesta vacía al aprovisionar el tenant.");

        return new BootstrapTenantResponse(result.MasterUserId, result.ActivationEmailSent);
    }

    public async Task SeedDemoDataAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsync(
            $"api/internal/provisioning/tenants/{tenantId}/seed-demo-data", content: null, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
