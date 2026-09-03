using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.Billing.CancelSubscription;

public class CancelSubscriptionCommandHandler(ISubscriptionRepository subscriptions, IUnitOfWork unitOfWork, IDateTime dateTime)
    : ICommandHandler<CancelSubscriptionCommand, CancelSubscriptionResult>
{
    public async Task<CancelSubscriptionResult> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptions.GetByIdAsync(command.SubscriptionId, cancellationToken)
            ?? throw new SubscriptionNotFoundException(command.SubscriptionId);

        subscription.Cancel(dateTime.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CancelSubscriptionResult(subscription.Id, subscription.Status.ToString());
    }
}
