using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.Billing.CancelSubscription;

/// <summary>CLAUDE.md §11 ("Cancelación") — cancelación inmediata, sin grace period (D18 pendiente).</summary>
public sealed record CancelSubscriptionCommand(Guid SubscriptionId) : ICommand<CancelSubscriptionResult>;
