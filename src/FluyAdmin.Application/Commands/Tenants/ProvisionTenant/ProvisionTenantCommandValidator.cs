using FluentValidation;

namespace FluyAdmin.Application.Commands.Tenants.ProvisionTenant;

public class ProvisionTenantCommandValidator : AbstractValidator<ProvisionTenantCommand>
{
    public ProvisionTenantCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);

        RuleFor(c => c.Subdomain)
            .NotEmpty()
            .Matches("^[a-z0-9][a-z0-9-]{1,61}[a-z0-9]$")
            .WithMessage("El subdominio debe tener entre 3 y 63 caracteres, solo minúsculas, números y guiones.");

        RuleFor(c => c.MasterEmail).NotEmpty().EmailAddress();
        RuleFor(c => c.MasterFullName).NotEmpty().MaximumLength(200);

        RuleFor(c => c.TrialDays)
            .InclusiveBetween(1, 3650)
            .When(c => c.TrialDays.HasValue)
            .WithMessage("La duración inicial debe ser de entre 1 y 3650 días.");
    }
}
