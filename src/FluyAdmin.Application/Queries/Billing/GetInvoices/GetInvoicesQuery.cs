using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Queries.Billing.GetInvoices;

public sealed record GetInvoicesQuery(Guid? TenantId) : IQuery<IReadOnlyCollection<InvoiceSummary>>;
