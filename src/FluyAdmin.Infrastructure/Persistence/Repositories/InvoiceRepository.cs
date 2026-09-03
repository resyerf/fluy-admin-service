using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Repositories;
using FluyAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FluyAdmin.Infrastructure.Persistence.Context;

namespace FluyAdmin.Infrastructure.Persistence.Repositories;

internal sealed class InvoiceRepository(PlatformDbContext db) : IInvoiceRepository
{
    public void Add(Invoice invoice) => db.Invoices.Add(invoice);

    public Task<Invoice?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Invoices.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<InvoiceDetail?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var invoice = await (
                from i in db.Invoices.AsNoTracking()
                join tenant in db.Tenants.AsNoTracking() on i.TenantId equals tenant.Id
                where i.Id == id
                select new InvoiceDetail(
                    i.Id, i.TenantId, tenant.Name, i.Number, i.Status.ToString(),
                    i.IssueDate, i.DueDate, i.TotalAmount, i.Currency, Array.Empty<PaymentDetail>()))
            .FirstOrDefaultAsync(cancellationToken);

        if (invoice is null)
        {
            return null;
        }

        var payments = await GetPaymentsAsync(id, cancellationToken);
        return invoice with { Payments = payments };
    }

    public async Task<IReadOnlyCollection<InvoiceSummary>> GetByTenantAsync(Guid? tenantId, CancellationToken cancellationToken) =>
        await (
                from i in db.Invoices.AsNoTracking()
                join tenant in db.Tenants.AsNoTracking() on i.TenantId equals tenant.Id
                where tenantId == null || i.TenantId == tenantId
                orderby i.IssueDate descending
                select new InvoiceSummary(i.Id, i.TenantId, tenant.Name, i.Number, i.Status.ToString(), i.IssueDate, i.DueDate, i.TotalAmount, i.Currency))
            .ToListAsync(cancellationToken);

    public void AddPayment(Payment payment) => db.Payments.Add(payment);

    public async Task<IReadOnlyCollection<PaymentDetail>> GetPaymentsAsync(Guid invoiceId, CancellationToken cancellationToken) =>
        await db.Payments.AsNoTracking().Where(p => p.InvoiceId == invoiceId)
            .Select(p => new PaymentDetail(p.Id, p.Amount, p.Currency, p.PaidAt, p.Method.ToString(), p.Reference))
            .ToListAsync(cancellationToken);

    public Task<decimal> GetTotalPaidAsync(Guid invoiceId, CancellationToken cancellationToken) =>
        db.Payments.AsNoTracking().Where(p => p.InvoiceId == invoiceId).SumAsync(p => p.Amount, cancellationToken);
}
