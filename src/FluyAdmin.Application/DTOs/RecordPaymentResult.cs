namespace FluyAdmin.Application.DTOs;

public record RecordPaymentResult(Guid PaymentId, Guid InvoiceId, string InvoiceStatus);
