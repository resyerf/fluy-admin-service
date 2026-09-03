using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Billing.GetSubscriptions;

public class GetSubscriptionsQueryHandler(ISubscriptionRepository subscriptions)
    : IQueryHandler<GetSubscriptionsQuery, IReadOnlyCollection<SubscriptionSummary>>
{
    public Task<IReadOnlyCollection<SubscriptionSummary>> Handle(GetSubscriptionsQuery query, CancellationToken cancellationToken) =>
        subscriptions.GetAllAsync(cancellationToken);
}
