using FluyAdmin.Application.Common.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Billing.GetSubscriptions;

public class GetSubscriptionsQueryHandler(ISubscriptionRepository subscriptions)
    : IQueryHandler<GetSubscriptionsQuery, IReadOnlyCollection<SubscriptionSummary>>
{
    public Task<IReadOnlyCollection<SubscriptionSummary>> Handle(GetSubscriptionsQuery query, CancellationToken cancellationToken) =>
        subscriptions.GetAllAsync(cancellationToken);
}
