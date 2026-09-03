using Fluy.SharedKernel.Dispatching;
using FluyAdmin.Application.DTOs;

namespace FluyAdmin.Application.Commands.Tenants.ProvisionTenant;

/// <summary>PlanCode nulo/vacío se resuelve a "FREE" en el handler. TrialDays nulo se resuelve a 30.</summary>
public sealed record ProvisionTenantCommand(
    string Name, string Subdomain, string MasterEmail, string MasterFullName, string? PlanCode = null, int? TrialDays = null)
    : ICommand<ProvisionTenantResult>;
