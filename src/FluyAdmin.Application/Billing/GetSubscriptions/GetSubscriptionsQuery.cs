using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Billing.GetSubscriptions;

public record GetSubscriptionsQuery : IQuery<IReadOnlyCollection<SubscriptionSummary>>;

public record SubscriptionSummary(
    Guid Id, Guid TenantId, string TenantName, string TenantSubdomain,
    string PlanCode, string PlanName, string Status,
    DateTimeOffset StartDate, DateTimeOffset? TrialEndsAt, DateTimeOffset? EndDate);
