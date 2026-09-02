using FluyAdmin.Domain.Billing;
using FluyAdmin.Domain.PlatformIdentity;
using Fluy.SharedKernel.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FluyAdmin.Infrastructure.Persistence;

/// <summary>
/// Bootstrap del primer PlatformUser (CODE.md §9.7) y siembra del catálogo de Features/Plans
/// (CLAUDE.md §9-§10) — ambos corren en cualquier entorno: sin PlatformUser nadie entra a
/// fluy-admin-web, y sin al menos el plan "FREE" ningún tenant puede aprovisionarse.
///
/// El catálogo de planes/features de abajo es un borrador técnico para poder cablear
/// Entitlements de punta a punta — la estructura comercial real (precios, nombres, límites)
/// es una decisión de negocio pendiente (CLAUDE.md §40), no algo que este código deba fijar.
/// </summary>
public class PlatformDbContextInitializer(
    PlatformDbContext context,
    IPasswordHasher passwordHasher,
    IConfiguration configuration,
    ILogger<PlatformDbContextInitializer> logger)
{
    private static readonly (string Code, string Name, FeatureType Type)[] FeatureCatalog =
    [
        ("workflow.basic", "Workflows básicos", FeatureType.Boolean),
        ("workflow.advanced", "Workflows avanzados", FeatureType.Boolean),
        ("rules.basic", "Motor de reglas básico", FeatureType.Boolean),
        ("rules.advanced", "Motor de reglas avanzado", FeatureType.Boolean),
        ("audit.basic", "Auditoría básica", FeatureType.Boolean),
        ("audit.advanced", "Auditoría avanzada", FeatureType.Boolean),
        ("api", "Acceso a API", FeatureType.Boolean),
        ("webhooks", "Webhooks", FeatureType.Boolean),
        ("sso", "Single Sign-On", FeatureType.Boolean),
        ("teams", "Integración con Microsoft Teams", FeatureType.Boolean),
        ("blockchain", "Evidencia en blockchain", FeatureType.Boolean),
        ("advanced.reporting", "Reportes avanzados", FeatureType.Boolean),
        ("max.users", "Usuarios máximos", FeatureType.Limit),
        ("max.branches", "Sedes máximas", FeatureType.Limit),
        ("max.workflows", "Workflows máximos", FeatureType.Limit),
        ("max.requests.month", "Solicitudes máximas por mes", FeatureType.Limit),
        ("max.storage.gb", "Almacenamiento máximo (GB)", FeatureType.Limit)
    ];

    private static readonly (string Code, string Name)[] PlanCatalog =
    [
        ("FREE", "FLUY Free"),
        ("STARTER", "FLUY Starter"),
        ("BUSINESS", "FLUY Business"),
        ("ENTERPRISE", "FLUY Enterprise")
    ];

    private static readonly Dictionary<string, Dictionary<string, string>> PlanFeatureMatrix = new()
    {
        ["FREE"] = new()
        {
            ["workflow.basic"] = "true", ["workflow.advanced"] = "false",
            ["rules.basic"] = "false", ["rules.advanced"] = "false",
            ["audit.basic"] = "true", ["audit.advanced"] = "false",
            ["api"] = "false", ["webhooks"] = "false", ["sso"] = "false",
            ["teams"] = "false", ["blockchain"] = "false", ["advanced.reporting"] = "false",
            ["max.users"] = "5", ["max.branches"] = "1", ["max.workflows"] = "3",
            ["max.requests.month"] = "100", ["max.storage.gb"] = "1"
        },
        ["STARTER"] = new()
        {
            ["workflow.basic"] = "true", ["workflow.advanced"] = "false",
            ["rules.basic"] = "true", ["rules.advanced"] = "false",
            ["audit.basic"] = "true", ["audit.advanced"] = "false",
            ["api"] = "false", ["webhooks"] = "false", ["sso"] = "false",
            ["teams"] = "false", ["blockchain"] = "false", ["advanced.reporting"] = "false",
            ["max.users"] = "25", ["max.branches"] = "3", ["max.workflows"] = "15",
            ["max.requests.month"] = "1000", ["max.storage.gb"] = "10"
        },
        ["BUSINESS"] = new()
        {
            ["workflow.basic"] = "true", ["workflow.advanced"] = "true",
            ["rules.basic"] = "true", ["rules.advanced"] = "true",
            ["audit.basic"] = "true", ["audit.advanced"] = "true",
            ["api"] = "true", ["webhooks"] = "true", ["sso"] = "false",
            ["teams"] = "true", ["blockchain"] = "false", ["advanced.reporting"] = "true",
            ["max.users"] = "100", ["max.branches"] = "10", ["max.workflows"] = "100",
            ["max.requests.month"] = "10000", ["max.storage.gb"] = "100"
        },
        ["ENTERPRISE"] = new()
        {
            ["workflow.basic"] = "true", ["workflow.advanced"] = "true",
            ["rules.basic"] = "true", ["rules.advanced"] = "true",
            ["audit.basic"] = "true", ["audit.advanced"] = "true",
            ["api"] = "true", ["webhooks"] = "true", ["sso"] = "true",
            ["teams"] = "true", ["blockchain"] = "true", ["advanced.reporting"] = "true",
            ["max.users"] = "unlimited", ["max.branches"] = "unlimited", ["max.workflows"] = "unlimited",
            ["max.requests.month"] = "unlimited", ["max.storage.gb"] = "unlimited"
        }
    };

    public async Task InitialiseAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.MigrateAsync(cancellationToken);
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedSuperAdminAsync(cancellationToken);
        await SeedBillingCatalogAsync(cancellationToken);
    }

    private async Task SeedSuperAdminAsync(CancellationToken cancellationToken)
    {
        var anyPlatformUser = await context.PlatformUsers.AnyAsync(cancellationToken);
        if (anyPlatformUser)
        {
            return;
        }

        var email = configuration["PlatformAdmin:Email"];
        var password = configuration["PlatformAdmin:InitialPassword"];
        var fullName = configuration["PlatformAdmin:FullName"] ?? "Platform SuperAdmin";

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning(
                "No hay ningún PlatformUser y no se configuró PlatformAdmin:Email/PlatformAdmin:InitialPassword " +
                "(o las variables de entorno PLATFORMADMIN__EMAIL/PLATFORMADMIN__INITIALPASSWORD). " +
                "fluy-admin-web no tendrá con qué usuario iniciar sesión.");
            return;
        }

        var superAdmin = PlatformUser.Create(email, fullName, passwordHasher.Hash(password), PlatformRole.SuperAdmin);
        context.PlatformUsers.Add(superAdmin);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("PlatformUser SuperAdmin sembrado: '{Email}'.", superAdmin.Email);
    }

    private async Task SeedBillingCatalogAsync(CancellationToken cancellationToken)
    {
        var anyPlan = await context.Plans.AnyAsync(cancellationToken);
        if (anyPlan)
        {
            return;
        }

        var features = FeatureCatalog.ToDictionary(
            f => f.Code,
            f => Feature.Create(f.Code, f.Name, f.Type));
        context.Features.AddRange(features.Values);

        var plans = PlanCatalog.ToDictionary(p => p.Code, p => Plan.Create(p.Code, p.Name));
        context.Plans.AddRange(plans.Values);

        await context.SaveChangesAsync(cancellationToken);

        foreach (var (planCode, featureValues) in PlanFeatureMatrix)
        {
            var plan = plans[planCode];
            foreach (var (featureCode, value) in featureValues)
            {
                context.PlanFeatures.Add(PlanFeature.Create(plan.Id, features[featureCode].Id, value));
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Catálogo de billing sembrado: {FeatureCount} features, {PlanCount} planes.",
            features.Count, plans.Count);
    }
}
