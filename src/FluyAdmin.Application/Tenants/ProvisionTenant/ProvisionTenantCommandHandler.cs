using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.Common.Interfaces;
using FluyAdmin.Application.Common.Interfaces.Repositories;
using FluyAdmin.Domain.Billing;
using FluyAdmin.Domain.Tenants;
using Fluy.SharedKernel;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Tenants.ProvisionTenant;

/// <summary>
/// Implementa el flujo de CODE.md §9.5. Si BootstrapTenantAsync falla, el tenant queda guardado
/// en estado PendingSetup (ya persistido antes de la llamada) y la excepción se propaga — nunca
/// se marca Active sin un usuario master funcional. La Subscription se crea en Trial, con la
/// duración pedida (command.TrialDays) o 30 días por defecto, sobre el plan pedido (o "FREE").
/// </summary>
public class ProvisionTenantCommandHandler(
    ITenantRepository tenants,
    IPlanRepository plans,
    ISubscriptionRepository subscriptions,
    IUnitOfWork unitOfWork,
    IProvisioningClient provisioningClient,
    IDateTime dateTime) : ICommandHandler<ProvisionTenantCommand, ProvisionTenantResult>
{
    private const int DefaultTrialDays = 30;

    public async Task<ProvisionTenantResult> Handle(ProvisionTenantCommand command, CancellationToken cancellationToken)
    {
        var subdomain = command.Subdomain.Trim().ToLowerInvariant();

        var subdomainTaken = await tenants.SubdomainTakenAsync(subdomain, cancellationToken);
        if (subdomainTaken)
        {
            throw new SubdomainAlreadyTakenException(subdomain);
        }

        var planCode = string.IsNullOrWhiteSpace(command.PlanCode) ? "FREE" : command.PlanCode.Trim().ToUpperInvariant();
        var plan = await plans.GetActiveByCodeAsync(planCode, cancellationToken)
            ?? throw new PlanNotFoundException(planCode);

        var tenant = Tenant.Create(command.Name, subdomain);
        tenants.Add(tenant);

        var trialLength = TimeSpan.FromDays(command.TrialDays ?? DefaultTrialDays);
        var subscription = Subscription.StartTrial(tenant.Id, plan.Id, dateTime.UtcNow, trialLength);
        subscriptions.Add(subscription);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var bootstrapResult = await provisioningClient.BootstrapTenantAsync(
            tenant.Id, command.MasterEmail, command.MasterFullName, cancellationToken);

        tenant.Activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ProvisionTenantResult(
            tenant.Id, bootstrapResult.MasterUserId, bootstrapResult.ActivationEmailSent,
            subscription.Id, plan.Code, subscription.TrialEndsAt!.Value);
    }
}
