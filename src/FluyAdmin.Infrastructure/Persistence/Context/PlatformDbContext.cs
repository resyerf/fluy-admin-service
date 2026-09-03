using System.Reflection;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Domain.Entities;
using Fluy.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace FluyAdmin.Infrastructure.Persistence.Context;

public class PlatformDbContext(DbContextOptions<PlatformDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<PlatformUser> PlatformUsers => Set<PlatformUser>();

    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<PlanFeature> PlanFeatures => Set<PlanFeature>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Schema "platform" (CODE.md §9.4) — comparte la misma instancia de Postgres que
        // fluy-service (schema "tenant"), pero cada servicio gestiona sus propias migraciones.
        modelBuilder.HasDefaultSchema("platform");

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
