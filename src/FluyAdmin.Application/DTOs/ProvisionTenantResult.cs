namespace FluyAdmin.Application.DTOs;

/// <summary>El email de activación del usuario master lo envía fluy-service directamente (CODE.md §9.22).</summary>
public record ProvisionTenantResult(
    Guid TenantId, Guid MasterUserId, bool ActivationEmailSent, Guid SubscriptionId, string PlanCode, DateTimeOffset TrialEndsAt);
