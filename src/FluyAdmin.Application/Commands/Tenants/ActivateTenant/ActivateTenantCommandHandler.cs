using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.Tenants.ActivateTenant;

public class ActivateTenantCommandHandler(ITenantRepository tenants, IUnitOfWork unitOfWork)
    : ICommandHandler<ActivateTenantCommand, ActivateTenantResult>
{
    public async Task<ActivateTenantResult> Handle(ActivateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await tenants.GetByIdAsync(command.TenantId, cancellationToken)
            ?? throw new TenantNotFoundException(command.TenantId);

        tenant.Activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ActivateTenantResult(tenant.Id, tenant.Status.ToString());
    }
}
