using Fluy.SharedKernel;

namespace FluyAdmin.Domain.Tenants;

public class Tenant : AggregateRoot, IAuditableEntity
{
    public string Name { get; private set; } = null!;
    public string Subdomain { get; private set; } = null!;
    public TenantStatus Status { get; private set; }

    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    private Tenant()
    {
    }

    public static Tenant Create(string name, string subdomain)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del tenant es obligatorio.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(subdomain))
        {
            throw new ArgumentException("El subdominio del tenant es obligatorio.", nameof(subdomain));
        }

        return new Tenant
        {
            Name = name.Trim(),
            Subdomain = subdomain.Trim().ToLowerInvariant(),
            Status = TenantStatus.PendingSetup
        };
    }

    public void Activate() => Status = TenantStatus.Active;

    public void Suspend() => Status = TenantStatus.Suspended;
}
