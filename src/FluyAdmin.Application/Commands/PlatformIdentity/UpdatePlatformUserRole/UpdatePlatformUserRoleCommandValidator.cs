using FluentValidation;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Application.Commands.PlatformIdentity.UpdatePlatformUserRole;

public class UpdatePlatformUserRoleCommandValidator : AbstractValidator<UpdatePlatformUserRoleCommand>
{
    public UpdatePlatformUserRoleCommandValidator()
    {
        RuleFor(c => c.PlatformUserId).NotEmpty();
        RuleFor(c => c.NewRole).NotEmpty().Must(r => Enum.TryParse<PlatformRole>(r, ignoreCase: true, out _))
            .WithMessage("El rol debe ser SuperAdmin, BillingOps, Support o ReadOnly.");
    }
}
