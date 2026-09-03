using FluyAdmin.Api.Models.Requests;
using FluyAdmin.Application.Commands.Billing.CancelSubscription;
using FluyAdmin.Application.Commands.Billing.ChangePlan;
using FluyAdmin.Application.Commands.Billing.IssueInvoice;
using FluyAdmin.Application.Commands.Billing.RecordPayment;
using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Queries.Billing.GetInvoices;
using FluyAdmin.Application.Queries.Billing.GetPlans;
using FluyAdmin.Application.Queries.Billing.GetSubscriptions;
using Fluy.SharedKernel.Dispatching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluyAdmin.Api.Controllers;

/// <summary>Billing (CLAUDE.md §12-§13) — reflejo local gestionado manualmente, sin proveedor de pagos conectado (CODE.md §4.15).</summary>
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

    [HttpPost("subscriptions/{subscriptionId:guid}/change-plan")]
    public async Task<ActionResult<ChangePlanResult>> ChangePlan(
        Guid subscriptionId, ChangePlanRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new ChangePlanCommand(subscriptionId, request.NewPlanCode), cancellationToken);
            return Ok(result);
        }
        catch (SubscriptionNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
        catch (PlanNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }

    [HttpPost("subscriptions/{subscriptionId:guid}/cancel")]
    public async Task<ActionResult<CancelSubscriptionResult>> CancelSubscription(Guid subscriptionId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new CancelSubscriptionCommand(subscriptionId), cancellationToken);
            return Ok(result);
        }
        catch (SubscriptionNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }

    [HttpGet("invoices")]
    public async Task<ActionResult<IReadOnlyCollection<InvoiceSummary>>> GetInvoices(
        [FromQuery] Guid? tenantId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetInvoicesQuery(tenantId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("invoices")]
    public async Task<ActionResult<IssueInvoiceResult>> IssueInvoice(IssueInvoiceRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(
                new IssueInvoiceCommand(request.SubscriptionId, request.TotalAmount, request.Currency, request.DueDate),
                cancellationToken);
            return Ok(result);
        }
        catch (SubscriptionNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }

    [HttpPost("invoices/{invoiceId:guid}/payments")]
    public async Task<ActionResult<RecordPaymentResult>> RecordPayment(
        Guid invoiceId, RecordPaymentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(
                new RecordPaymentCommand(invoiceId, request.Amount, request.Currency, request.Method, request.Reference),
                cancellationToken);
            return Ok(result);
        }
        catch (InvoiceNotFoundException ex)
        {
            return NotFound(new { detail = ex.Message });
        }
    }
}
