namespace FluyAdmin.Api.Models.Requests;

public record RecordPaymentRequest(decimal Amount, string Currency, string Method, string? Reference);
