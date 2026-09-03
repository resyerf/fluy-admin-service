using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Queries.Billing.GetInvoices;

public class GetInvoicesQueryHandler(IInvoiceRepository invoices)
    : IQueryHandler<GetInvoicesQuery, IReadOnlyCollection<InvoiceSummary>>
{
    public Task<IReadOnlyCollection<InvoiceSummary>> Handle(GetInvoicesQuery query, CancellationToken cancellationToken) =>
        invoices.GetByTenantAsync(query.TenantId, cancellationToken);
}
