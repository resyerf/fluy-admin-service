using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.Billing.IssueInvoice;

/// <summary>CLAUDE.md §12 ("Facturas") — reflejo local de una factura emitida manualmente (sin proveedor de billing, CODE.md §4.15).</summary>
public sealed record IssueInvoiceCommand(Guid SubscriptionId, decimal TotalAmount, string Currency, DateTimeOffset DueDate)
    : ICommand<IssueInvoiceResult>;
