using Fluy.SharedKernel;

namespace FluyAdmin.Domain.Entities;

/// <summary>
/// Línea contratada dentro de una suscripción (ej. un add-on por fuera del plan base). Entidad
/// independiente con SubscriptionId como FK plano, mismo patrón que RequestField en fluy-service:
/// se crea y consulta vía el repositorio del padre (ISubscriptionRepository), sin colección
/// navegable desde Subscription.
/// </summary>
public class SubscriptionItem : BaseEntity
{
    public Guid SubscriptionId { get; private set; }
    public Guid FeatureId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private SubscriptionItem()
    {
    }

    public static SubscriptionItem Create(Guid subscriptionId, Guid featureId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(quantity));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(unitPrice));
        }

        return new SubscriptionItem
        {
            SubscriptionId = subscriptionId,
            FeatureId = featureId,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}
