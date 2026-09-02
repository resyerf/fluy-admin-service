using FluyAdmin.Application.Billing.GetPlans;
using FluyAdmin.Domain.Billing;

namespace FluyAdmin.Application.Common.Interfaces.Repositories;

public interface IPlanRepository
{
    Task<Plan?> GetActiveByCodeAsync(string code, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<PlanSummary>> GetAllWithFeaturesAsync(CancellationToken cancellationToken);
}
