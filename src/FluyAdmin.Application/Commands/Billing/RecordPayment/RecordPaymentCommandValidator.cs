using FluentValidation;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Application.Commands.Billing.RecordPayment;

public class RecordPaymentCommandValidator : AbstractValidator<RecordPaymentCommand>
{
    public RecordPaymentCommandValidator()
    {
        RuleFor(c => c.InvoiceId).NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
        RuleFor(c => c.Currency).NotEmpty().Length(3);
        RuleFor(c => c.Method).NotEmpty().Must(m => Enum.TryParse<PaymentMethod>(m, ignoreCase: true, out _))
            .WithMessage("El método de pago debe ser Manual, BankTransfer, Card u Other.");
    }
}
