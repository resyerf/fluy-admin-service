namespace FluyAdmin.Application.DTOs;

public record PaymentDetail(Guid Id, decimal Amount, string Currency, DateTimeOffset PaidAt, string Method, string? Reference);
