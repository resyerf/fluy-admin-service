namespace FluyAdmin.Application.DTOs;

public record TenantSummary(Guid Id, string Name, string Subdomain, string Status, DateTimeOffset CreatedAt);
