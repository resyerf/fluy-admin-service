using FluentValidation;

namespace FluyAdmin.Application.Commands.Billing.ChangePlan;

public class ChangePlanCommandValidator : AbstractValidator<ChangePlanCommand>
{
    public ChangePlanCommandValidator()
    {
        RuleFor(c => c.SubscriptionId).NotEmpty();
        RuleFor(c => c.NewPlanCode).NotEmpty().MaximumLength(50);
    }
}
