using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Usage.GetUsageForTenant;

/// <summary>Period nulo se resuelve al mes calendario actual (yyyy-MM) en el handler.</summary>
public sealed record GetUsageForTenantQuery(Guid TenantId, string? Period) : IQuery<IReadOnlyCollection<UsageSummary>>;
