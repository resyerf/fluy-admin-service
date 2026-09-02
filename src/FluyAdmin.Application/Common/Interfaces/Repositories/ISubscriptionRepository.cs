using FluyAdmin.Application.Billing.GetSubscriptions;
using FluyAdmin.Domain.Billing;

namespace FluyAdmin.Application.Common.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    void Add(Subscription subscription);
    Task<IReadOnlyCollection<SubscriptionSummary>> GetAllAsync(CancellationToken cancellationToken);
}
