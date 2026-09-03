using FluyAdmin.Application.Commands.Tenants.ProvisionTenant;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Application.Interfaces.Services;
using Fluy.SharedKernel.Dispatching;
using Microsoft.Extensions.Logging;

namespace FluyAdmin.Infrastructure.Persistence;

/// <summary>
/// Siembra un tenant demo completo (subdominio "demo", login usuario@demo.com/clavedemo123) solo en
/// Development, para poder ver FLUY funcionando sin aprovisionar nada a mano. Reusa el flujo real de
/// producción (ProvisionTenantCommand) en vez de escribir Tenant/Subscription a mano — así el demo
/// nunca se desincroniza de cómo se aprovisiona un tenant de verdad. El resto de la data de negocio
/// (organización, workflow, solicitudes de ejemplo) la puebla fluy-service, dueño de ese schema, vía
/// IProvisioningClient.SeedDemoDataAsync — igual que BootstrapTenantAsync ya hace para el usuario
/// master.
///
/// Separada de PlatformDbContextInitializer a propósito: esa clase siembra lo que debe existir en
/// *cualquier* entorno (SuperAdmin, catálogo de billing); esto es una conveniencia exclusiva de
/// Development que además depende de que fluy-service esté alcanzable por HTTP — si no lo está,
/// se loguea y no tumba el arranque de fluy-admin-service (mismo criterio que ActivationEmailSender).
/// </summary>
public class DemoTenantSeeder(
    ITenantRepository tenants,
    ISender sender,
    IProvisioningClient provisioningClient,
    ILogger<DemoTenantSeeder> logger)
{
    private const string DemoSubdomain = "demo";

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var subdomainTaken = await tenants.SubdomainTakenAsync(DemoSubdomain, cancellationToken);
        if (subdomainTaken)
        {
            return;
        }

        try
        {
            var result = await sender.Send(
                new ProvisionTenantCommand(
                    Name: "Empresa Demo S.A.",
                    Subdomain: DemoSubdomain,
                    MasterEmail: "usuario@demo.com",
                    MasterFullName: "Usuario Demo",
                    PlanCode: "BUSINESS",
                    TrialDays: 365),
                cancellationToken);

            await provisioningClient.SeedDemoDataAsync(result.TenantId, cancellationToken);

            logger.LogInformation("Tenant demo sembrado (subdominio '{Subdomain}').", DemoSubdomain);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "No se pudo sembrar el tenant demo — ¿fluy-service está corriendo y alcanzable? " +
                "Reiniciar fluy-admin-service una vez que lo esté para reintentar.");
        }
    }
}
