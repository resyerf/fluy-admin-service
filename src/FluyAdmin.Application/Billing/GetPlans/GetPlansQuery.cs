using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Billing.GetPlans;

public record GetPlansQuery : IQuery<IReadOnlyCollection<PlanSummary>>;

public record PlanFeatureSummary(string FeatureCode, string FeatureName, string FeatureType, string Value);

public record PlanSummary(Guid Id, string Code, string Name, bool IsActive, IReadOnlyCollection<PlanFeatureSummary> Features);
