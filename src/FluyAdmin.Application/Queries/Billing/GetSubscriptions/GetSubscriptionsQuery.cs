using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Billing.GetSubscriptions;

public record GetSubscriptionsQuery : IQuery<IReadOnlyCollection<SubscriptionSummary>>;
