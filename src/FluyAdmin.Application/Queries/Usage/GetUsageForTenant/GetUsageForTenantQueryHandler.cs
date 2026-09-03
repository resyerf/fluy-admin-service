using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Queries.Usage.GetUsageForTenant;

public class GetUsageForTenantQueryHandler(IUsageRepository usage, IDateTime dateTime)
    : IQueryHandler<GetUsageForTenantQuery, IReadOnlyCollection<UsageSummary>>
{
    public Task<IReadOnlyCollection<UsageSummary>> Handle(GetUsageForTenantQuery query, CancellationToken cancellationToken)
    {
        var period = string.IsNullOrWhiteSpace(query.Period) ? dateTime.UtcNow.ToString("yyyy-MM") : query.Period;
        return usage.GetForTenantAsync(query.TenantId, period, cancellationToken);
    }
}
