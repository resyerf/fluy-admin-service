using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.Commands.Tenants.ActivateTenant;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Queries.Tenants.GetTenants;
using FluyAdmin.Application.Commands.Tenants.ProvisionTenant;
using FluyAdmin.Application.Commands.Tenants.SuspendTenant;
using Fluy.SharedKernel.Dispatching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluyAdmin.Api.Models.Requests;

namespace FluyAdmin.Api.Controllers;

[ApiController]
[Route("api/v1/tenants")]
[Authorize]
public class TenantsController(ISender sender) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TenantSummary>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTenantsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProvisionTenantResult>> Provision(
        ProvisionTenantRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(
                new ProvisionTenantCommand(
                    request.Name, request.Subdomain, request.MasterEmail, request.MasterFullName, request.PlanCode, request.TrialDays),
                cancellationToken);

            return Ok(result);
        }
        catch (SubdomainAlreadyTakenException ex)
        {
            return Conflict(new { detail = ex.Message });
        }
        catch (PlanNotFoundException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }

    [HttpPost("{id:guid}/suspend")]
    public async Task<ActionResult<SuspendTenantResult>> Suspend(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new SuspendTenantCommand(id), cancellationToken);
            return Ok(result);
        }
        catch (TenantNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult<ActivateTenantResult>> Activate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new ActivateTenantCommand(id), cancellationToken);
            return Ok(result);
        }
        catch (TenantNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }
}
