using Fluy.SharedKernel;
using FluyAdmin.Domain.Enums;

namespace FluyAdmin.Domain.Entities;

/// <summary>
/// TenantId es un FK simple aquí, no un eje de particionamiento — a diferencia de fluy-service,
/// FluyAdmin.Domain no tiene el concepto de ITenantEntity (CODE.md §9.3): Tenant es una fila más
/// de este mismo dominio, no algo por lo que haya que filtrar las consultas de FluyAdmin.
/// </summary>
public class Subscription : AggregateRoot, IAuditableEntity
{
    public Guid TenantId { get; private set; }
    public Guid PlanId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset? TrialEndsAt { get; private set; }
    public DateTimeOffset? EndDate { get; private set; }

    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    private Subscription()
    {
    }

    public static Subscription StartTrial(Guid tenantId, Guid planId, DateTimeOffset now, TimeSpan trialLength) => new()
    {
        TenantId = tenantId,
        PlanId = planId,
        Status = SubscriptionStatus.Trial,
        StartDate = now,
        TrialEndsAt = now.Add(trialLength)
    };

    public void Activate() => Status = SubscriptionStatus.Active;

    public void Suspend() => Status = SubscriptionStatus.Suspended;

    public void Cancel(DateTimeOffset now)
    {
        Status = SubscriptionStatus.Cancelled;
        EndDate = now;
    }
}
