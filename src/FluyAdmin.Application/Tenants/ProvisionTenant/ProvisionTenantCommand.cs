using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Tenants.ProvisionTenant;

/// <summary>PlanCode nulo/vacío se resuelve a "FREE" en el handler. TrialDays nulo se resuelve a 30.</summary>
public record ProvisionTenantCommand(
    string Name, string Subdomain, string MasterEmail, string MasterFullName, string? PlanCode = null, int? TrialDays = null)
    : ICommand<ProvisionTenantResult>;
