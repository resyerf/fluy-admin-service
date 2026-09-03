using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Tenants.GetTenants;

public class GetTenantsQueryHandler(ITenantRepository tenants) : IQueryHandler<GetTenantsQuery, IReadOnlyCollection<TenantSummary>>
{
    public Task<IReadOnlyCollection<TenantSummary>> Handle(GetTenantsQuery query, CancellationToken cancellationToken) =>
        tenants.GetAllAsync(cancellationToken);
}
