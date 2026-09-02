using Fluy.SharedKernel;

namespace FluyAdmin.Domain.Billing;

/// <summary>
/// Value es siempre string para no forzar el modelo a elegir entre número/booleano a nivel de
/// columna: "true"/"false" para features Boolean, un número o "unlimited" para features Limit.
/// La interpretación queda del lado de quien consume (EntitlementReader en fluy-service).
/// </summary>
public class PlanFeature : BaseEntity
{
    public Guid PlanId { get; private set; }
    public Guid FeatureId { get; private set; }
    public string Value { get; private set; } = null!;

    private PlanFeature()
    {
    }

    public static PlanFeature Create(Guid planId, Guid featureId, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("El valor del feature es obligatorio.", nameof(value));
        }

        return new PlanFeature
        {
            PlanId = planId,
            FeatureId = featureId,
            Value = value.Trim()
        };
    }
}
