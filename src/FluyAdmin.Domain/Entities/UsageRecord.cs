using Fluy.SharedKernel;

namespace FluyAdmin.Domain.Entities;

/// <summary>
/// Contador de consumo real de un tenant para un período. MetricCode correlaciona directo con
/// Feature.Code (los de FeatureType.Limit, ej. "max.requests.month") — no existe una entidad
/// UsageMetric separada porque Feature ya es el catálogo de métricas/límites (CLAUDE.md §13).
/// fluy-service incrementa esta tabla vía UPDATE/upsert cross-schema (CODE.md §9.4, excepción #3),
/// nunca crea/borra el catálogo de Feature.
/// </summary>
public class UsageRecord : BaseEntity
{
    public Guid TenantId { get; private set; }
    public string MetricCode { get; private set; } = null!;
    public string Period { get; private set; } = null!;
    public long Count { get; private set; }

    private UsageRecord()
    {
    }

    public static UsageRecord Create(Guid tenantId, string metricCode, string period)
    {
        if (string.IsNullOrWhiteSpace(metricCode))
        {
            throw new ArgumentException("El código de la métrica es obligatorio.", nameof(metricCode));
        }

        if (string.IsNullOrWhiteSpace(period))
        {
            throw new ArgumentException("El período es obligatorio.", nameof(period));
        }

        return new UsageRecord { TenantId = tenantId, MetricCode = metricCode.Trim(), Period = period.Trim(), Count = 0 };
    }

    public void Increment(long amount) => Count += amount;
}
