using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Billing.GetPlans;

public class GetPlansQueryHandler(IPlanRepository plans) : IQueryHandler<GetPlansQuery, IReadOnlyCollection<PlanSummary>>
{
    public Task<IReadOnlyCollection<PlanSummary>> Handle(GetPlansQuery query, CancellationToken cancellationToken) =>
        plans.GetAllWithFeaturesAsync(cancellationToken);
}
