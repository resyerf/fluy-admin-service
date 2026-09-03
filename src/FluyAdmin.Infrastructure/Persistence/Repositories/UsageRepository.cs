using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Entities;
using FluyAdmin.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using FluyAdmin.Infrastructure.Persistence.Context;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class UsageRepository(PlatformDbContext db) : IUsageRepository
{
    public async Task IncrementAsync(Guid tenantId, string metricCode, string period, long amount, CancellationToken cancellationToken)
    {
        var record = await db.UsageRecords.FirstOrDefaultAsync(
            u => u.TenantId == tenantId && u.MetricCode == metricCode && u.Period == period, cancellationToken);

        if (record is null)
        {
            record = UsageRecord.Create(tenantId, metricCode, period);
            db.UsageRecords.Add(record);
        }

        record.Increment(amount);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<UsageSummary>> GetForTenantAsync(Guid tenantId, string period, CancellationToken cancellationToken)
    {
        var activeSubscription = await db.Subscriptions.AsNoTracking()
            .Where(s => s.TenantId == tenantId && s.Status != SubscriptionStatus.Cancelled && s.Status != SubscriptionStatus.Expired)
            .OrderByDescending(s => s.StartDate)
            .FirstOrDefaultAsync(cancellationToken);

        var limits = activeSubscription is null
            ? []
            : await (
                    from planFeature in db.PlanFeatures.AsNoTracking()
                    join feature in db.Features.AsNoTracking() on planFeature.FeatureId equals feature.Id
                    where planFeature.PlanId == activeSubscription.PlanId && feature.Type == FeatureType.Limit
                    select new { feature.Code, feature.Name, planFeature.Value })
                .ToListAsync(cancellationToken);

        var counts = await db.UsageRecords.AsNoTracking()
            .Where(u => u.TenantId == tenantId && u.Period == period)
            .ToDictionaryAsync(u => u.MetricCode, u => u.Count, cancellationToken);

        return limits
            .Select(limit => new UsageSummary(limit.Code, limit.Name, counts.GetValueOrDefault(limit.Code), limit.Value))
            .ToList();
    }
}
