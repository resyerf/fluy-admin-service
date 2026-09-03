using Fluy.SharedKernel;

namespace FluyAdmin.Domain.Entities;

public class Plan : BaseEntity, IAuditableEntity
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }

    public Guid? CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    private Plan()
    {
    }

    public static Plan Create(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("El código del plan es obligatorio.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del plan es obligatorio.", nameof(name));
        }

        return new Plan
        {
            Code = code.Trim().ToUpperInvariant(),
            Name = name.Trim(),
            IsActive = true
        };
    }

    public void Deactivate() => IsActive = false;
}
