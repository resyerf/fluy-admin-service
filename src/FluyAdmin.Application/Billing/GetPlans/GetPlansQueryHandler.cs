using FluyAdmin.Application.Common.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Billing.GetPlans;

public class GetPlansQueryHandler(IPlanRepository plans) : IQueryHandler<GetPlansQuery, IReadOnlyCollection<PlanSummary>>
{
    public Task<IReadOnlyCollection<PlanSummary>> Handle(GetPlansQuery query, CancellationToken cancellationToken) =>
        plans.GetAllWithFeaturesAsync(cancellationToken);
}
