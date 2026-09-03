using FluyAdmin.Application.Common.Exceptions;
using FluyAdmin.Application.DTOs;
using FluyAdmin.Application.Interfaces.Services;
using FluyAdmin.Application.Interfaces.Repositories;
using Fluy.SharedKernel.Dispatching;

namespace FluyAdmin.Application.Commands.Tenants.SuspendTenant;

public class SuspendTenantCommandHandler(ITenantRepository tenants, IUnitOfWork unitOfWork)
    : ICommandHandler<SuspendTenantCommand, SuspendTenantResult>
{
    public async Task<SuspendTenantResult> Handle(SuspendTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await tenants.GetByIdAsync(command.TenantId, cancellationToken)
            ?? throw new TenantNotFoundException(command.TenantId);

        tenant.Suspend();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SuspendTenantResult(tenant.Id, tenant.Status.ToString());
    }
}
