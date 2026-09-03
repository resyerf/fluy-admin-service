namespace FluyAdmin.Application.DTOs;

public record InvoiceDetail(
    Guid Id, Guid TenantId, string TenantName, string Number, string Status,
    DateTimeOffset IssueDate, DateTimeOffset DueDate, decimal TotalAmount, string Currency,
    IReadOnlyCollection<PaymentDetail> Payments);
