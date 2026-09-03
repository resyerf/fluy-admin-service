namespace FluyAdmin.Application.DTOs;

public record UsageSummary(string MetricCode, string MetricName, long Used, string? Limit);
