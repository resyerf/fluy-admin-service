using Fluy.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FluyAdmin.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Análogo al de fluy-service, simplificado: todavía no existe un ICurrentPlatformUserService
/// (no hay endpoints protegidos que lo requieran aún), así que solo se completan las marcas de
/// tiempo; CreatedBy/LastModifiedBy quedan pendientes hasta que exista ese port.
/// </summary>
public class AuditableEntitiesInterceptor(IDateTime dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = dateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedAt = dateTime.UtcNow;
                    break;
            }
        }
    }
}
