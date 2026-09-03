using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Infrastructure.Persistence;
using FluyAdmin.Infrastructure.Persistence.Context;
using FluyAdmin.Infrastructure.Persistence.Interceptors;
using FluyAdmin.Infrastructure.Persistence.Repositories;
using FluyAdmin.Infrastructure.External.Clients.Http;
using Fluy.SharedKernel;
using Fluy.SharedKernel.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluyAdmin.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitiesInterceptor>();

        services.AddDbContext<PlatformDbContext>((sp, options) =>
        {
            // Historial de migraciones propio en el schema "platform" (simétrico a lo que hace
            // fluy-service para "tenant", CODE.md §10-D15) — misma instancia de Postgres, sin choque.
            options.UseNpgsql(
                configuration.GetConnectionString("PlatformDb"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "platform"));
            options.AddInterceptors(sp.GetRequiredService<AuditableEntitiesInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<PlatformDbContext>());
        services.AddScoped<PlatformDbContextInitializer>();
        services.AddScoped<DemoTenantSeeder>();

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IPlatformUserRepository, PlatformUserRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IUsageRepository, UsageRepository>();

        services.AddSingleton<IDateTime, SystemDateTime>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.Configure<FluyServiceProvisioningOptions>(configuration.GetSection(FluyServiceProvisioningOptions.SectionName));
        services.AddHttpClient<IProvisioningClient, FluyServiceProvisioningClient>((sp, client) =>
        {
            var options = configuration.GetSection(FluyServiceProvisioningOptions.SectionName).Get<FluyServiceProvisioningOptions>()
                ?? new FluyServiceProvisioningOptions();

            client.BaseAddress = new Uri(options.BaseUrl);
            client.DefaultRequestHeaders.Add("X-Service-Api-Key", options.ApiKey);
        });

        return services;
    }
}
