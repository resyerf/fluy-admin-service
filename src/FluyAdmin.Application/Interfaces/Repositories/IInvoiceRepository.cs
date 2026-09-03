using FluyAdmin.Application.DTOs;
using FluyAdmin.Domain.Entities;

namespace FluyAdmin.Application.Interfaces.Repositories;

public interface IInvoiceRepository
{
    void Add(Invoice invoice);
    Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<InvoiceDetail?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<InvoiceSummary>> GetByTenantAsync(Guid? tenantId, CancellationToken cancellationToken);
    void AddPayment(Payment payment);
    Task<IReadOnlyCollection<PaymentDetail>> GetPaymentsAsync(Guid invoiceId, CancellationToken cancellationToken);
    Task<decimal> GetTotalPaidAsync(Guid invoiceId, CancellationToken cancellationToken);
}
