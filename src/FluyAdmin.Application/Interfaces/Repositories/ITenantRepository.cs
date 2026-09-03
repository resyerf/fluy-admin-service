using FluyAdmin.Application.DTOs;
using FluyAdmin.Domain.Entities;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface ITenantRepository
{
    Task<bool> SubdomainTakenAsync(string subdomain, CancellationToken cancellationToken);
    void Add(Tenant tenant);
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TenantSummary>> GetAllAsync(CancellationToken cancellationToken);
}
