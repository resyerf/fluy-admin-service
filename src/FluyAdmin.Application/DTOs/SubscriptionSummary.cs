namespace FluyAdmin.Application.DTOs;

public record SubscriptionSummary(
    Guid Id, Guid TenantId, string TenantName, string TenantSubdomain,
    string PlanCode, string PlanName, string Status,
    DateTimeOffset StartDate, DateTimeOffset? TrialEndsAt, DateTimeOffset? EndDate);
