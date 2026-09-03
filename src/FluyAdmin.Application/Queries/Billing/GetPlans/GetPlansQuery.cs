using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Billing.GetPlans;

public sealed record GetPlansQuery : IQuery<IReadOnlyCollection<PlanSummary>>;
