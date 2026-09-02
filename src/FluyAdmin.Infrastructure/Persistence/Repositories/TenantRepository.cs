using FluyAdmin.Application.Common.Interfaces.Repositories;
using FluyAdmin.Application.Tenants.GetTenants;
using FluyAdmin.Domain.Tenants;
using Microsoft.EntityFrameworkCore;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class TenantRepository(PlatformDbContext db) : ITenantRepository
{
    public Task<bool> SubdomainTakenAsync(string subdomain, CancellationToken cancellationToken) =>
        db.Tenants.AnyAsync(t => t.Subdomain == subdomain, cancellationToken);

    public void Add(Tenant tenant) => db.Tenants.Add(tenant);

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Tenants.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<TenantSummary>> GetAllAsync(CancellationToken cancellationToken) =>
        await db.Tenants.AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TenantSummary(t.Id, t.Name, t.Subdomain, t.Status.ToString(), t.CreatedAt))
            .ToListAsync(cancellationToken);
}
