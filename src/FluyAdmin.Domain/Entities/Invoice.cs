using Fluy.SharedKernel;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Domain.Entities;

/// <summary>
/// Reflejo local de una factura (CLAUDE.md §12) — FLUY es responsable de este registro, no del
/// cobro real: sin proveedor de billing conectado (CODE.md §4.15), se emite y se marca pagada
/// manualmente por un PlatformUser (rol BillingOps) vía RecordPaymentCommand.
/// </summary>
public class Invoice : AggregateRoot, IAuditableEntity
{
    public Guid TenantId { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public string Number { get; private set; } = null!;
    public DateTimeOffset IssueDate { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Currency { get; private set; } = null!;

    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    private Invoice()
    {
    }

    public static Invoice Create(
        Guid tenantId, Guid subscriptionId, string number, DateTimeOffset issueDate, DateTimeOffset dueDate,
        decimal totalAmount, string currency)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("El número de factura es obligatorio.", nameof(number));
        }

        if (totalAmount < 0)
        {
            throw new ArgumentException("El monto total no puede ser negativo.", nameof(totalAmount));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("La moneda es obligatoria.", nameof(currency));
        }

        return new Invoice
        {
            TenantId = tenantId,
            SubscriptionId = subscriptionId,
            Number = number.Trim(),
            IssueDate = issueDate,
            DueDate = dueDate,
            Status = InvoiceStatus.Draft,
            TotalAmount = totalAmount,
            Currency = currency.Trim().ToUpperInvariant()
        };
    }

    public void Issue() => Status = InvoiceStatus.Issued;

    public void MarkPaid() => Status = InvoiceStatus.Paid;

    public void MarkPastDue() => Status = InvoiceStatus.PastDue;

    public void Void() => Status = InvoiceStatus.Void;
}
