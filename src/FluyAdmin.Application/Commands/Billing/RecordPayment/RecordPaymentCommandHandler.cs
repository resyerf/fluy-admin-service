using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Entities;
using FluyAdmin.Domain.Enums;
using Fluy.SharedKernel;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.Billing.RecordPayment;

public class RecordPaymentCommandHandler(IInvoiceRepository invoices, IUnitOfWork unitOfWork, IDateTime dateTime)
    : ICommandHandler<RecordPaymentCommand, RecordPaymentResult>
{
    public async Task<RecordPaymentResult> Handle(RecordPaymentCommand command, CancellationToken cancellationToken)
    {
        var invoice = await invoices.GetByIdAsync(command.InvoiceId, cancellationToken)
            ?? throw new InvoiceNotFoundException(command.InvoiceId);

        var method = Enum.Parse<PaymentMethod>(command.Method, ignoreCase: true);
        var payment = Payment.Create(invoice.Id, command.Amount, command.Currency, dateTime.UtcNow, method, command.Reference);
        invoices.AddPayment(payment);

        var totalPaidSoFar = await invoices.GetTotalPaidAsync(invoice.Id, cancellationToken);
        if (totalPaidSoFar + command.Amount >= invoice.TotalAmount)
        {
            invoice.MarkPaid();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RecordPaymentResult(payment.Id, invoice.Id, invoice.Status.ToString());
    }
}
