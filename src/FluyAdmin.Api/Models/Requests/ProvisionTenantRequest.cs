namespace FluyAdmin.Api.Models.Requests;

public record ProvisionTenantRequest(
    string Name, string Subdomain, string MasterEmail, string MasterFullName, string? PlanCode, int? TrialDays);
