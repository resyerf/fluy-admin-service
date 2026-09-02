using FluyAdmin.Application.Billing.GetPlans;
using FluyAdmin.Application.Common.Interfaces.Repositories;
using FluyAdmin.Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class PlanRepository(PlatformDbContext db) : IPlanRepository
{
    public Task<Plan?> GetActiveByCodeAsync(string code, CancellationToken cancellationToken) =>
        db.Plans.FirstOrDefaultAsync(p => p.Code == code && p.IsActive, cancellationToken);

    public async Task<IReadOnlyCollection<PlanSummary>> GetAllWithFeaturesAsync(CancellationToken cancellationToken)
    {
        var plans = await db.Plans.AsNoTracking().OrderBy(p => p.Name).ToListAsync(cancellationToken);

        var features = await (
                from planFeature in db.PlanFeatures.AsNoTracking()
                join feature in db.Features.AsNoTracking() on planFeature.FeatureId equals feature.Id
                select new
                {
                    planFeature.PlanId,
                    FeatureSummary = new PlanFeatureSummary(feature.Code, feature.Name, feature.Type.ToString(), planFeature.Value)
                })
            .ToListAsync(cancellationToken);

        return plans
            .Select(p => new PlanSummary(
                p.Id, p.Code, p.Name, p.IsActive,
                features.Where(f => f.PlanId == p.Id).Select(f => f.FeatureSummary).OrderBy(f => f.FeatureCode).ToList()))
            .ToList();
    }
}
