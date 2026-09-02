using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Tenants.GetTenants;

public record GetTenantsQuery : IQuery<IReadOnlyCollection<TenantSummary>>;

public record TenantSummary(Guid Id, string Name, string Subdomain, string Status, DateTimeOffset CreatedAt);
