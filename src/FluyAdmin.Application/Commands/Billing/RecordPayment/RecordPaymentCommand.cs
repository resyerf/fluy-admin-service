using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.Billing.RecordPayment;

/// <summary>CLAUDE.md §12 ("Pagos") — registro manual de un pago (sin proveedor de billing, CODE.md §4.15). Method: Manual|BankTransfer|Card|Other.</summary>
public sealed record RecordPaymentCommand(Guid InvoiceId, decimal Amount, string Currency, string Method, string? Reference)
    : ICommand<RecordPaymentResult>;
