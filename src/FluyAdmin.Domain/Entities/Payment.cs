using Fluy.SharedKernel;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Domain.Entities;

/// <summary>
/// Registro de un pago contra una factura. Sin proveedor de billing conectado (CODE.md §4.15),
/// se registra manualmente — Method distingue cómo se recibió el pago, no un proveedor real.
/// </summary>
public class Payment : BaseEntity, IAuditableEntity
{
    public Guid InvoiceId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public DateTimeOffset PaidAt { get; private set; }
    public PaymentMethod Method { get; private set; }
    public string? Reference { get; private set; }

    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    private Payment()
    {
    }

    public static Payment Create(
        Guid invoiceId, decimal amount, string currency, DateTimeOffset paidAt, PaymentMethod method, string? reference)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("El monto del pago debe ser mayor a cero.", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("La moneda es obligatoria.", nameof(currency));
        }

        return new Payment
        {
            InvoiceId = invoiceId,
            Amount = amount,
            Currency = currency.Trim().ToUpperInvariant(),
            PaidAt = paidAt,
            Method = method,
            Reference = string.IsNullOrWhiteSpace(reference) ? null : reference.Trim()
        };
    }
}
