using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Entities;
using Fluy.SharedKernel;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.Billing.IssueInvoice;

public class IssueInvoiceCommandHandler(ISubscriptionRepository subscriptions, IInvoiceRepository invoices, IUnitOfWork unitOfWork, IDateTime dateTime)
    : ICommandHandler<IssueInvoiceCommand, IssueInvoiceResult>
{
    public async Task<IssueInvoiceResult> Handle(IssueInvoiceCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptions.GetByIdAsync(command.SubscriptionId, cancellationToken)
            ?? throw new SubscriptionNotFoundException(command.SubscriptionId);

        var now = dateTime.UtcNow;
        var number = $"INV-{now:yyyyMM}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        var invoice = Invoice.Create(subscription.TenantId, subscription.Id, number, now, command.DueDate, command.TotalAmount, command.Currency);
        invoice.Issue();
        invoices.Add(invoice);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new IssueInvoiceResult(invoice.Id, invoice.Number, invoice.Status.ToString());
    }
}
