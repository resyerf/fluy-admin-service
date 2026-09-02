using FluyAdmin.Application.Common.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Tenants.GetTenants;

public class GetTenantsQueryHandler(ITenantRepository tenants) : IQueryHandler<GetTenantsQuery, IReadOnlyCollection<TenantSummary>>
{
    public Task<IReadOnlyCollection<TenantSummary>> Handle(GetTenantsQuery query, CancellationToken cancellationToken) =>
        tenants.GetAllAsync(cancellationToken);
}
