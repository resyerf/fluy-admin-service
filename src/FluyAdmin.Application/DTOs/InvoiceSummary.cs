namespace FluyAdmin.Application.DTOs;

public record InvoiceSummary(
    Guid Id, Guid TenantId, string TenantName, string Number, string Status,
    DateTimeOffset IssueDate, DateTimeOffset DueDate, decimal TotalAmount, string Currency);
