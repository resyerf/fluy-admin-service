namespace FluyAdmin.Application.DTOs;

public record PlanSummary(Guid Id, string Code, string Name, bool IsActive, IReadOnlyCollection<PlanFeatureSummary> Features);
