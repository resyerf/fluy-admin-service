using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface IUsageRepository
{
    Task IncrementAsync(Guid tenantId, string metricCode, string period, long amount, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<UsageSummary>> GetForTenantAsync(Guid tenantId, string period, CancellationToken cancellationToken);
}
