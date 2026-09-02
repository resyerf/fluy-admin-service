using FluyAdmin.Application.Tenants.GetTenants;
using FluyAdmin.Domain.Tenants;

namespace FluyAdmin.Application.Common.Interfaces.Repositories;

public interface ITenantRepository
{
    Task<bool> SubdomainTakenAsync(string subdomain, CancellationToken cancellationToken);
    void Add(Tenant tenant);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TenantSummary>> GetAllAsync(CancellationToken cancellationToken);
}
