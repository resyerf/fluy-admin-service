using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Tenants.GetTenants;

public sealed record GetTenantsQuery : IQuery<IReadOnlyCollection<TenantSummary>>;
