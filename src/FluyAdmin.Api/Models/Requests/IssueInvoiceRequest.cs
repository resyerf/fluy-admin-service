namespace FluyAdmin.Api.Models.Requests;

public record IssueInvoiceRequest(Guid SubscriptionId, decimal TotalAmount, string Currency, DateTimeOffset DueDate);
