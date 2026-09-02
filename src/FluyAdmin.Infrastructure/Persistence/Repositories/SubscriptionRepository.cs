using FluyAdmin.Application.Billing.GetSubscriptions;
using FluyAdmin.Application.Common.Interfaces.Repositories;
using FluyAdmin.Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class SubscriptionRepository(PlatformDbContext db) : ISubscriptionRepository
{
    public void Add(Subscription subscription) => db.Subscriptions.Add(subscription);

    public async Task<IReadOnlyCollection<SubscriptionSummary>> GetAllAsync(CancellationToken cancellationToken) =>
        await (
                from subscription in db.Subscriptions.AsNoTracking()
                join tenant in db.Tenants.AsNoTracking() on subscription.TenantId equals tenant.Id
                join plan in db.Plans.AsNoTracking() on subscription.PlanId equals plan.Id
                orderby subscription.StartDate descending
                select new SubscriptionSummary(
                    subscription.Id, tenant.Id, tenant.Name, tenant.Subdomain,
                    plan.Code, plan.Name, subscription.Status.ToString(),
                    subscription.StartDate, subscription.TrialEndsAt, subscription.EndDate))
            .ToListAsync(cancellationToken);
}
