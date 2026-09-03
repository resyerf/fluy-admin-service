using FluyAdmin.Application.Queries.Billing.GetPlans;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Queries.Billing.GetSubscriptions;
using Fluy.SharedKernel.Dispatching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluyAdmin.Api.Controllers;

/// <summary>Lectura del catálogo de Billing (CLAUDE.md §12-§13) — sin comandos de cambio de plan/pago todavía (CODE.md §4.15).</summary>
[ApiController]
[Route("api/v1/billing")]
[Authorize]
public class BillingController(ISender sender) : ControllerBase
{
    [HttpGet("plans")]
    public async Task<ActionResult<IReadOnlyCollection<PlanSummary>>> GetPlans(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPlansQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("subscriptions")]
    public async Task<ActionResult<IReadOnlyCollection<SubscriptionSummary>>> GetSubscriptions(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSubscriptionsQuery(), cancellationToken);
        return Ok(result);
    }
}
