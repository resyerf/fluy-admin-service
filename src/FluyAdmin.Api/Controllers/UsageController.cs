using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Queries.Usage.GetUsageForTenant;
using Fluy.SharedKernel.Dispatching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluyAdmin.Api.Controllers;

/// <summary>Consumo real por tenant (CLAUDE.md §13) — correlaciona contra los límites del catálogo de Billing.</summary>
[ApiController]
[Route("api/v1/usage")]
[Authorize]
public class UsageController(ISender sender) : ControllerBase
{
    [HttpGet("tenants/{tenantId:guid}")]
    public async Task<ActionResult<IReadOnlyCollection<UsageSummary>>> GetForTenant(
        Guid tenantId, [FromQuery] string? period, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUsageForTenantQuery(tenantId, period), cancellationToken);
        return Ok(result);
    }
}
