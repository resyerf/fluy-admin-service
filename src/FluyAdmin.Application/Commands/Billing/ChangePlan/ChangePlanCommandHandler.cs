using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.Billing.ChangePlan;

public class ChangePlanCommandHandler(ISubscriptionRepository subscriptions, IPlanRepository plans, IUnitOfWork unitOfWork)
    : ICommandHandler<ChangePlanCommand, ChangePlanResult>
{
    public async Task<ChangePlanResult> Handle(ChangePlanCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptions.GetByIdAsync(command.SubscriptionId, cancellationToken)
            ?? throw new SubscriptionNotFoundException(command.SubscriptionId);

        var planCode = command.NewPlanCode.Trim().ToUpperInvariant();
        var plan = await plans.GetActiveByCodeAsync(planCode, cancellationToken) ?? throw new PlanNotFoundException(planCode);

        subscription.ChangePlan(plan.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ChangePlanResult(subscription.Id, plan.Code);
    }
}
