using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.Billing.ChangePlan;

/// <summary>CLAUDE.md §11 ("Cambio de plan") — upgrade/downgrade inmediato, sin prorrateo (D18 pendiente).</summary>
public sealed record ChangePlanCommand(Guid SubscriptionId, string NewPlanCode) : ICommand<ChangePlanResult>;
