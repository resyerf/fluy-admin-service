using FluentValidation;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Application.Commands.PlatformIdentity.CreatePlatformUser;

public class CreatePlatformUserCommandValidator : AbstractValidator<CreatePlatformUserCommand>
{
    public CreatePlatformUserCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.FullName).NotEmpty().MaximumLength(200);
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);
        RuleFor(c => c.Role).NotEmpty().Must(r => Enum.TryParse<PlatformRole>(r, ignoreCase: true, out _))
            .WithMessage("El rol debe ser SuperAdmin, BillingOps, Support o ReadOnly.");
    }
}
